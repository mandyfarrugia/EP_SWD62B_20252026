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
    }
}