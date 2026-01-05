using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Services
{
    public class BlackFridayPromotion : ICalculatingTotal
    {
        private IBooksRepository _booksRepository;

        public BlackFridayPromotion(IBooksRepository booksRepository)
        {
            this._booksRepository = booksRepository;
        }

        public double Calculate(List<OrderItem> orderItems)
        {
            double total = 0;

            foreach (OrderItem orderItem in orderItems)
            {
                Book book = this._booksRepository.Get(orderItem.BookFK);
                total += (book.WholesalePrice * orderItem.Qty);
            }

            return total * .5; //Apply 50% discount.
        }
    }
}
