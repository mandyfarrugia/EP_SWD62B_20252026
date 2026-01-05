using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class BackupController : Controller
    {
        //Injecting two different implementations, inheriting from the same interface.
        public IActionResult Backup(
            [FromKeyedServices("db")] IBooksRepository booksDbRepository,
            [FromKeyedServices("file")] IBooksRepository booksFileRepository)
        {
            List<Book> listOfBooksFromDatabase = booksDbRepository.Get().ToList();

            foreach(Book book in listOfBooksFromDatabase)
            {
                booksFileRepository.Add(book);
            }

            return Content("Backup taken");
        }
    }
}
