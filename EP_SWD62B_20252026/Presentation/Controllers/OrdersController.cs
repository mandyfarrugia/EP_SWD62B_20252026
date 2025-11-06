using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class OrdersController : Controller
    {
        private OrdersRepository _ordersRepository;
        private BooksRepository _booksRepository;
        private ICalculatingTotal _calculationService;

        //Constructor Injection: Injecting application service called OrdersRepository.
        public OrdersController(OrdersRepository ordersRepository, BooksRepository booksRepository, ICalculatingTotal calculationService)
        {
            this._ordersRepository = ordersRepository;
            this._booksRepository = booksRepository;
            this._calculationService = calculationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Buy(List<OrderItem> orderItems)
        {
            Order order = new Order();
            order.DatePlaced = DateTime.Now;
            this._ordersRepository.Checkout(order, orderItems, this._booksRepository);
            double finalTotal = this._calculationService.Calculate(orderItems);
            TempData["success"] = $"Final total withdrawn is {finalTotal}. Books bought successfully.";
            return RedirectToAction("Index", "Books"); //How to redirect to an action inside another controller.
        }
    }
}