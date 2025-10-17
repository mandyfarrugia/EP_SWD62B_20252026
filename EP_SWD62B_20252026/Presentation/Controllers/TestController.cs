using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            TestModel testModel = new TestModel()
            {
                Author = User.Identity.IsAuthenticated ? User.Identity.Name : "Me",
                Message = "This is a test. Welcome to ASP.NET Core site.",
                DatePublished = DateTime.Now
            };

            return View(testModel);
        }

        public IActionResult SubmitForm(TestModel data)
        {
            if (data != null)
            {
                //Process the data.
                if (!string.IsNullOrEmpty(data.Message))
                {
                    int wordCount = data.Message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length; //Do not count empty entries.
                    ViewBag.WordCount = wordCount;
                }

                if(!string.IsNullOrEmpty(data.Author))
                {
                    data.Author = data.Author.ToUpper();
                }
                
                data.DatePublished = DateTime.Now;

                return View(nameof(Index), data); //Otherwise, by default, the controller will attempt to redirect the user to a view named SubmitForm which does not exist.
            }

            return View(nameof(Index));
        }
    }
}
