using DataAccess.Context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    //This will manage journals to/from database
    public class JournalsRepository
    {
        private ShoppingCartDbContext _context;

        public JournalsRepository(ShoppingCartDbContext context)
        {
            this._context = context;
        }

        public void Add(Journal journal) 
        {
            this._context.Journals.Add(journal);
            this._context.SaveChanges();
        }
    }
}
