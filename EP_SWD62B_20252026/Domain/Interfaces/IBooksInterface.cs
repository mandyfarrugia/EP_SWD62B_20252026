using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBooksRepository
    {
        IQueryable<Book> Get(); //read
        Book Get(int id);
        void Add(Book book); //write
    }
}
