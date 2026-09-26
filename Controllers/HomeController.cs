using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharjahEventsWeb.Models;

namespace SharjahEventsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchQuery)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.SearchQuery = searchQuery;

            try
            {
                var bookFairs = _context.SharjahBookFairs.ToList();
                var childFestivals = _context.SharjahChildFestivals.ToList();
                var distributors = _context.DistributorsConferences.ToList();
                var newYork = _context.NewYorkSessions.ToList();
                var publishersConf = _context.PublishersConferences.ToList();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    bookFairs = bookFairs.Where(x => 
                        (x.PublishingHouseName != null && x.PublishingHouseName.Contains(searchQuery)) || 
                        (x.Country != null && x.Country.Contains(searchQuery)) || 
                        (x.City != null && x.City.Contains(searchQuery))).ToList();

                    childFestivals = childFestivals.Where(x => 
                        (x.PublishingHouseName != null && x.PublishingHouseName.Contains(searchQuery)) || 
                        (x.Country != null && x.Country.Contains(searchQuery)) || 
                        (x.City != null && x.City.Contains(searchQuery))).ToList();

                    distributors = distributors.Where(x => 
                        (x.PublishingHouseName != null && x.PublishingHouseName.Contains(searchQuery)) || 
                        (x.Country != null && x.Country.Contains(searchQuery)) || 
                        (x.City != null && x.City.Contains(searchQuery))).ToList();

                    newYork = newYork.Where(x => 
                        (x.PublishingHouseName != null && x.PublishingHouseName.Contains(searchQuery)) || 
                        (x.Country != null && x.Country.Contains(searchQuery)) || 
                        (x.City != null && x.City.Contains(searchQuery))).ToList();

                    publishersConf = publishersConf.Where(x => 
                        (x.PublishingHouseName != null && x.PublishingHouseName.Contains(searchQuery)) || 
                        (x.Country != null && x.Country.Contains(searchQuery)) || 
                        (x.City != null && x.City.Contains(searchQuery))).ToList();
                }

                ViewBag.BookFairs = bookFairs;
                ViewBag.ChildFestivals = childFestivals;
                ViewBag.Distributors = distributors;
                ViewBag.NewYork = newYork;
                ViewBag.PublishersConf = publishersConf;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                ViewBag.BookFairs = new List<SharjahBookFair>();
                ViewBag.ChildFestivals = new List<SharjahChildFestival>();
                ViewBag.Distributors = new List<DistributorsConference>();
                ViewBag.NewYork = new List<NewYorkSession>();
                ViewBag.PublishersConf = new List<PublishersConference>();
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", email);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}