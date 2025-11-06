using DataAccess.Context;
using Domain.Models;

namespace DataAccess.Repositories
{
    public class OrdersRepository
    {
        private ShoppingCartDbContext _context;

        public OrdersRepository(ShoppingCartDbContext context)
        {
            this._context = context;
        }

        private void AddOrderItem(OrderItem orderItem)
        {
            this._context.OrderItems.Add(orderItem);
            this._context.SaveChanges();
        }

        private void AddOrder(Order order)
        {
            this._context.Orders.Add(order);
            this._context.SaveChanges();
        }

        //Checkout creates a structured insert and contains validation.
        public void Checkout(Order order, List<OrderItem> orderItems, BooksRepository booksRepository)
        {
            //Activate promotion period.

            order.Id = Guid.NewGuid(); //Control the ID of the order.

            this.AddOrder(order); //Add the order to the database.

            foreach (OrderItem orderItem in orderItems)
            {
                orderItem.OrderFK = order.Id; //They compose the same order. Set the generated ID into each of the OrderItem instance.
                Book book = booksRepository.Get(orderItem.BookFK); //Get the book from the database to check stock.

                //Checking the stock whether I have more than the requested quantity.
                if (book.Stock >= orderItem.Qty)
                {
                    this.AddOrderItem(orderItem);
                    book.Stock -= orderItem.Qty; //Reduce stock.
                    booksRepository.UpdateBook(book);
                }
            }
        }
    }
}