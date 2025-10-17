using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class BooksController : Controller
    {
        //Constructor Injection is one of the variations of Dependency Injection.
        private BooksRepository _booksRepository;

        public BooksController(BooksRepository booksRepository)
        {
            this._booksRepository = booksRepository;
        }

        public IActionResult Index()
        {
            IQueryable<Book> books = this._booksRepository.Get();
            return View();
        }

        [HttpGet] //Loads and renders a page with empty input controls.
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //Handles the submission form.
        public IActionResult Create(Book book)
        {
            this._booksRepository.Add(book);
            return View(book);
        }

        public IActionResult Delete(int id)
        {
            this._booksRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
