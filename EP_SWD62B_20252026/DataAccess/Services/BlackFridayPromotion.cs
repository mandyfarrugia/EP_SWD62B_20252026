using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class BlackFridayPromotion : ICalculatingTotal
    {
        private BooksRepository _booksRepository;

        public BlackFridayPromotion(BooksRepository booksRepository)
        {
            this._booksRepository = booksRepository;
        }

        public double Calculate(List<OrderItem> orderItems)
        {
            double total = 0;

            foreach (OrderItem orderItem in orderItems)
            {
                Book book = this._booksRepository.Get(orderItem.BookFK);
                total += book.WholesalePrice;
            }

            return total * .5; //Apply 50% discount.
        }
    }
}
