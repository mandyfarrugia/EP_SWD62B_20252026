using DataAccess.Repositories;
using Domain.Models;

namespace Presentation.Models
{
    /// <summary>
    /// ViewModels should only be used when there is the UI (user-interface) involved,
    /// whilst Models such as Book should be used to model the database (for migrations).
    /// </summary>
    public class BooksCreateViewModel
    {
        /// <summary>
        /// The CLR (Common Language Runtime) uses a default constructor to instantiate an object of this type when rendering the View.
        /// We can have an overloaded constructor such as the one below where we can pass the list of Category instances.
        /// </summary>
        /// <param name="categoriesRepository"></param>
        public BooksCreateViewModel()
        {
        }

        public Book Book { get; set; } //Hold the created book.
        public IFormFile UpdatedFile { get; set; } //In ASP.NET Core 6 and later versions, we receive files through the IFormFile interface.
        public List<Category> Categories { get; set; } //Must be populated beforehand.
    }
}