using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class BooksController : Controller
    {
        //Constructor Injection is one of the variations of Dependency Injection.
        private BooksRepository _booksRepository { get; set; }

        public BooksController(BooksRepository booksRepository)
        {
            this._booksRepository = booksRepository;
        }

        //Method Injection - Use [FromServices] BooksRepository _booksRepository in the parameter list.

        [HttpGet]
        public IActionResult Index()
        {
            /* IQueryable - prepares an SQL statement. 
             * ToList<> or AsEnumerable() - open a connection with the database. 
             * When you run ToList(), it also enables you to inspect the data. */
            List<Book> books = this._booksRepository.Get().ToList();
            return View(books); //A call will be initiated with the database.
        }

        [HttpPost]
        public IActionResult Index(string keyword)
        {
            List<Book> filteredList = this._booksRepository.Get(keyword).ToList();
            return View(filteredList);
        }

        public IActionResult Details(int id)
        {
            Book book = this._booksRepository.Get(id);
            return View(book);
        }

        [HttpGet] //Loads and renders a page with empty input controls.
        public IActionResult Create([FromServices] CategoriesRepository categoriesRepository)
        {
            /* BooksCreateViewModel 
             * - Book entity
             * - List of categories */

            //Fetched list of categories passed to the page.
            BooksCreateViewModel booksCreateViewModel = new BooksCreateViewModel();
            booksCreateViewModel.Categories = categoriesRepository.Get().ToList(); //A call to the database is done upon invoking ToList().
            return View(booksCreateViewModel); //Use a ViewModel as a carrier of data to the view.
        }

        [HttpPost] //Handles the submission form.

        /* We refer to services being injected as either Application Services or Framework Services. 
         * Application Services: CategoriesRepository (injecting user-defined classes that act as services that we have created ourselves)
         * Framework Services: IWebHostEnvironment (built-in services provided by .NET Core) */
        public IActionResult Create(BooksCreateViewModel booksCreateViewModel, [FromServices] CategoriesRepository categoriesRepository, [FromServices] IWebHostEnvironment host) //The model has to match the one being expected.
        {
            try
            {
                //We need to receive the image.
                if(booksCreateViewModel.UpdatedFile != null)
                {
                    //We have a file...

                    //File needs to be saved.
                    string uniqueFilename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(booksCreateViewModel.UpdatedFile.FileName);
                    string absolutePath = Path.Combine(host.WebRootPath, "images", uniqueFilename);

                    //FileStream is one of the methods available to save files into a server.
                    using(FileStream fileStream = new FileStream(absolutePath, FileMode.CreateNew))
                    {
                        booksCreateViewModel.UpdatedFile.CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Close();
                    }

                    booksCreateViewModel.Book.Path = Path.DirectorySeparatorChar + Path.Combine("images", uniqueFilename);
                }

                /* Ways how you can pass data from the server side (i.e. controller) to the client side (i.e. views):
                 * - TempData = It survives a redirection (if I redirect the user to Index page instead, TempData is still accessible)
                 * - ViewData or ViewBag = They are fine if there is no redirection and data is lost after you pass it to the page.
                 *    - ViewBag.success = "Book created successfully";
                 *    - ViewData["success"] = "Book created successfully";
                 * - Session = In session, whatever you store survives many redirections until the user logs out or until the user leaves the account unattended for x minutes.
                 * - Cookies = Cookies are files stored in the client browser - they may be used to store data BUT be careful - cookies are not encrypted.
                 * - Models = So we can edit the Book class, add a property called Feedback and we set it with the data we want to pass back to the page. */

                this._booksRepository.Add(booksCreateViewModel.Book);
                TempData["success"] = "Book created successfully.";
                return RedirectToAction(nameof(Create)); //Return a redirection where the request came from with no data related to the book - prevents reinstantiating BooksCreateViewModel and repopulating list of categories.
            }
            catch (Exception exception)
            {
                booksCreateViewModel.Categories = categoriesRepository.Get().ToList();
                TempData["failure"] = "Error occurred - Book was not saved. Try again, we are working on it.";
                return View(booksCreateViewModel); //Loading back the view where the request came from with the submitted data.
            }
        }

        [HttpGet]
        public IActionResult Update(int id, [FromServices] CategoriesRepository categoriesRepository) 
        {
            BooksCreateViewModel booksCreateViewModel = new BooksCreateViewModel();
            booksCreateViewModel.Categories = categoriesRepository.Get().ToList(); //A call to the database is done upon invoking ToList().
            booksCreateViewModel.Book = this._booksRepository.Get(id);
            return View(booksCreateViewModel);
        }

        [HttpPost]
        public IActionResult Update(BooksCreateViewModel booksCreateViewModel, [FromServices] CategoriesRepository categoriesRepository) 
        {
            try
            {
                TempData["success"] = "Book update successfully.";

                this._booksRepository.UpdateBook(booksCreateViewModel.Book);
                return RedirectToAction(nameof(Update), new { id = booksCreateViewModel.Book.Id });
            }
            catch (Exception exception)
            {
                booksCreateViewModel.Categories = categoriesRepository.Get().ToList();
                TempData["failure"] = "Error occurred - Book was not saved. Try again, we are working on it."; //Log the error.
                return View(booksCreateViewModel);
            }
        }

        public IActionResult Delete(int id)
        {
            this._booksRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int[] ids)
        {
            try
            {
                foreach (int id in ids)
                {
                    this._booksRepository.Delete(id);
                }

                TempData["success"] = "Book(s) deleted successfully!";
            }
            catch(Exception exception)
            {
                TempData["failure"] = "Books were not deleted. Try again!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
