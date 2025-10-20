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

        public IActionResult Index()
        {
            IQueryable<Book> books = this._booksRepository.Get();
            return View();
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
        public IActionResult Create(BooksCreateViewModel booksCreateViewModel, [FromServices] CategoriesRepository categoriesRepository) //The model has to match the one being expected.
        {
            try
            {
                /* Ways how you can pass data from the server side (i.e. controller) to the client side (i.e. views):
                 * - TempData = It survives a redirection (if I redirect the user to Index page instead, TempData is still accessible)
                 * - ViewData or ViewBag = They are fine if there is no redirection and data is lost after you pass it to the page.
                 *    - ViewBag.success = "Book created successfully";
                 *    - ViewData["success"] = "Book created successfully";
                 * - Session = In session, whatever you store survives many redirections until the user logs out or until the user leaves the account unattended for x minutes.
                 * - Cookies = Cookies are files stored in the client browser - they may be used to store data BUT be careful - cookies are not encrypted.
                 * - Models = So we can edit the Book class, add a property called Feedback and we set it with the data we want to pass back to the page. */

                TempData["success"] = "Book created successfully.";

                this._booksRepository.Add(booksCreateViewModel.Book);
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
    }
}
