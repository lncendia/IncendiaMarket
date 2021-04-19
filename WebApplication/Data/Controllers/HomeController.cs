using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Data.Interfaces;
using WebApplication.ViewModels;

namespace WebApplication.Data.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAllAdvertisement _cars;

        public HomeController(IAllAdvertisement cars)
        {
            _cars = cars;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ViewResult AboutUs()
        {
            return View();
        }
    }
}