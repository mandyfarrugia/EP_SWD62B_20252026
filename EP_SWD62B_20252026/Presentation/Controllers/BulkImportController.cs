using DataAccess.Repositories;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Factory;

namespace Presentation.Controllers
{
    public class BulkImportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BulkImport(
            string json,
            [FromKeyedServices("db")] IBooksRepository booksRepository,
            [FromServices] JournalsRepository journalsRepository)
        {
            string jsonTestData = @"{
  ""Title"": ""Clean Code"",
  ""WholesalePrice"": 34.99,
  ""PublishedYear"": 2008,
  ""CategoryFK"": 1,
  ""Stock"": 25,
  ""Path"": ""images/books/cleancode.jpg""
}";

            BookFactory bookFactory = new BookFactory();
            bookFactory.BuildAndSave(jsonTestData, booksRepository, journalsRepository);

            //Call the factory class to build Journal/Book out of the JSON passed.

            //So then they can be saved into the database.

            return View();
        }
    }
}
