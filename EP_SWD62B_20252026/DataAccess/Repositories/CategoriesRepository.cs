using DataAccess.Context;
using Domain.Models;

namespace DataAccess.Repositories
{
    public class CategoriesRepository
    {
        private ShoppingCartDbContext _context;

        public CategoriesRepository(ShoppingCartDbContext context)
        {
            this._context = context;
        }

        //IQueryable is a prepared statement. List/IEnumerable handles the call.
        public IQueryable<Category> Get()
        {
            return this._context.Categories;
        }

        public IQueryable<Category> Get(string keyword)
        {
            return this.Get().Where(category => category.Name.Contains(keyword));
        }

        public Category Get(int id)
        {
            return this.Get().SingleOrDefault(category => category.Id == id);
        }

        public void Add(Category category)
        {
            this._context.Categories.Add(category);
            this._context.SaveChanges(); //Persist everything into the database.
        }

        public void UpdateBook(Category category)
        {
            Category categoryToUpdate = this.Get(category.Id);

            if (categoryToUpdate != null)
            {
                categoryToUpdate.Name = category.Name;
                this._context.SaveChanges(); //Persist everything into the database.
            }
        }

        public void Delete(int id)
        {
            Category categoryToDelete = this.Get(id);

            if (categoryToDelete != null)
            {
                this._context.Categories.Remove(categoryToDelete);
                this._context.SaveChanges(); //Persist everything into the database.
            }
        }
    }
}
