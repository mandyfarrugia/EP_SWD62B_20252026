using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Services
{
    public class NoPromotion : ICalculatingTotal
    {
        private BooksRepository _booksRepository;

        public NoPromotion(BooksRepository booksRepository)
        {
            this._booksRepository = booksRepository;
        }

        public double Calculate(List<OrderItem> orderItems)
        {
            double total = 0;

            foreach(OrderItem orderItem in orderItems)
            {
                Book book = this._booksRepository.Get(orderItem.BookFK);
                total += book.WholesalePrice;
            }

            return total;
        }
    }
}