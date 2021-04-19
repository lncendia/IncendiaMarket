using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data.Interfaces;
using WebApplication.Data.Models;
using WebApplication.ViewModels;

namespace WebApplication.Data.Controllers
{
    [Route("Catalog")]
    public class CatalogController : Controller
    {
        private readonly IAllAdvertisement _adverts;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<IdentityUser> _userManager;

        public CatalogController(IAllAdvertisement cars, IWebHostEnvironment environment,
            UserManager<IdentityUser> userManager)
        {
            _adverts = cars;
            _environment = environment;
            _userManager = userManager;
        }

        [Route("List")]
        [HttpGet]
        public IActionResult List()
        {
            List<Advertisement> cars = _adverts.Advertisements.Include(_ => _.Category).Include(_ => _.IdentityUser)
                .OrderByDescending(_ => _.Date).Take(8).ToList();
            ViewBag.Title = "Каталог";
            return View(cars);
        }

        [Route("List")]
        [HttpPost]
        public IActionResult List(IFormCollection model)
        {
            if (!ModelState.IsValid) return NoContent();
            IQueryable<Advertisement> advertisements =
                _adverts.Advertisements.Include(_ => _.Category).Include(_ => _.IdentityUser);
            if (int.TryParse(model["minPrice"].FirstOrDefault(), out int minPrice))
            {
                advertisements = advertisements.Where(adv => adv.Price >= minPrice);
            }

            if (int.TryParse(model["maxPrice"].FirstOrDefault(), out int maxPrice))
            {
                advertisements = advertisements.Where(adv => adv.Price <= maxPrice);
            }

            if (model["categories"].Any())
            {
                List<int> categories = new List<int>();
                foreach (var x in model["categories"])
                {
                    if (int.TryParse(x, out int buffer)) categories.Add(buffer);
                }

                advertisements = advertisements
                    .Where(_ => categories.Contains(_.Category.Id));
            }

            if (model["sort"].FirstOrDefault() != null)
            {
                advertisements = model["sort"].FirstOrDefault() switch
                {
                    "date" => advertisements.OrderByDescending(adv => adv.Date),
                    "priceHigh" => advertisements.OrderByDescending(adv => adv.Price),
                    "priceLow" => advertisements.OrderBy(adv => adv.Price),
                    "countViews" => advertisements.OrderByDescending(adv => adv.CountViews),
                    _ => advertisements.OrderByDescending(adv => adv.Date)
                };
            }

            if (int.TryParse(model["page"].FirstOrDefault(), out int page))
            {
                advertisements = advertisements.Skip(page * 8);
            }

            advertisements = advertisements.Take(8);
            var allCars = advertisements.ToList();

            if (allCars.Count == 0) return NoContent();
            string layout = model["layout"].FirstOrDefault() == null ? "grid" : model["layout"];
            return PartialView("ListAJAX", new AdvertisementListViewModel {Advertisements = allCars, Layout = layout});
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            var adv = _adverts.GetAdvertisementWithAddingView(id);
            if (adv == null) return NotFound();
            var comments = _adverts.GetComments(adv, -1);
            return View(new AdvertisementInfoViewModel()
                {Advertisement = adv, Comments = comments, User = await _userManager.GetUserAsync(User)});
        }

        [HttpPost]
        [Route("{id:int}/Comments")]
        public async Task<IActionResult> Comments(int id, int page)
        {
            if (!ModelState.IsValid) return NoContent();
            var adv = _adverts.GetAdvertisement(id);
            if (adv is null) return NoContent();
            var comments = _adverts.GetComments(adv, page * 8);
            if (comments.Count == 0) return NoContent();
            var user = await _userManager.GetUserAsync(User);
            return PartialView("CommentsAJAX", new AdvertisementInfoViewModel() {Comments = comments, User = user});
        }

        [HttpPost]
        [Route("{id:int}/AddComment")]
        public async Task<IActionResult> AddComment(int id, string comment, int parentId)
        {
            try
            {
                if (User.Identity is {IsAuthenticated: false}) return Unauthorized();
                var adv = _adverts.GetAdvertisement(id);
                if (adv is null) return NoContent();
                var comments = _adverts.AddComment(await _userManager.GetUserAsync(User), adv, comment, parentId);
                var user = await _userManager.GetUserAsync(User);
                return PartialView("CommentsAJAX", new AdvertisementInfoViewModel() {Comments = comments, User = user});
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("DeleteComment")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var comment = _adverts.RemoveComment(id, user);
            if (comment)
                return Ok();
            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("AddAdvertisement")]
        public ViewResult AddAdvertisement()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [Route("AddAdvertisement")]
        public async Task<IActionResult> AddAdvertisement(Advertisement car, IFormFile file)
        {
            if (User.Identity is {IsAuthenticated: false}) return Unauthorized();
            if (!ModelState.IsValid || file == null) return View(car);
            var path = "/Files/" + file.Name;
            await using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            car.Image = path;
            if (_adverts.AddAdvertisement(car)) return RedirectToAction("Index", car.Id);
            return View(car);
        }
    }
}