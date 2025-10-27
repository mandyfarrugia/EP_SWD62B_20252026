using DataAccess.Context;
using Domain.Models;

namespace DataAccess.Repositories
{
    /* Repository classes will serve as raw CRUD methods to the database.
     * C - Create
     * R - Read
     * U - Update
     * D - Delete */

    /* IQueryable vs IEnumerable/List<T>
     * IQueryable = It does not execute the command - it prepares an SQL command.
     * IEnumerable = Every command is executed - it opens a connection. */

    public class BooksRepository
    {
        //This is called Construction Injection.
        private ShoppingCartDbContext _context;

        public BooksRepository(ShoppingCartDbContext context)
        {
            this._context = context;
        }

        public IQueryable<Book> Get()
        {
            return this._context.Books;
        }

        public IQueryable<Book> Get(string keyword)
        {
            return this.Get().Where(book => book.Title.Contains(keyword));
        }

        public Book Get(int id)
        {
            return this.Get().SingleOrDefault(book => book.Id == id);
        }

        public void Add(Book book)
        {
            this._context.Books.Add(book);
            this._context.SaveChanges(); //Persist everything into the database.
        }

        public void UpdateBook(Book book)
        {
            Book bookToUpdate = this.Get(book.Id);

            if (bookToUpdate != null)
            {
                bookToUpdate.Title = book.Title;
                bookToUpdate.PublishedYear = book.PublishedYear;
                bookToUpdate.WholesalePrice = book.WholesalePrice;
                bookToUpdate.Stock = book.Stock;
                bookToUpdate.CategoryFK = book.CategoryFK;
                bookToUpdate.Path = book.Path;

                this._context.SaveChanges(); //Persist everything into the database.
            }
        }

        public void Delete(int id)
        {
            Book bookToDelete = this.Get(id);

            if(bookToDelete != null)
            {
                this._context.Books.Remove(bookToDelete);
                this._context.SaveChanges(); //Persist everything into the database.
            }
        }
    }
}