using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharjahEventsWeb.Data;
using SharjahEventsWeb.Models;

namespace SharjahEventsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (email == "admin@sharjah.ae" && password == "Admin@2026")
            {
                HttpContext.Session.SetString("UserEmail", email);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserEmail");
            return RedirectToAction("Login");
        }

        public IActionResult Index(string searchQuery)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.SearchQuery = searchQuery;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                ViewBag.BookFairs = _context.SharjahBookFairs
                    .Where(x => x.PublishingHouseName.Contains(searchQuery) || x.Country.Contains(searchQuery) || x.City.Contains(searchQuery))
                    .ToList();

                ViewBag.ChildFestivals = _context.SharjahChildFestivals
                    .Where(x => x.PublishingHouseName.Contains(searchQuery) || x.Country.Contains(searchQuery) || x.City.Contains(searchQuery))
                    .ToList();

                ViewBag.Distributors = _context.DistributorsConferences
                    .Where(x => x.PublishingHouseName.Contains(searchQuery) || x.Country.Contains(searchQuery) || x.City.Contains(searchQuery))
                    .ToList();

                ViewBag.NewYork = _context.NewYorkSessions
                    .Where(x => x.PublishingHouseName.Contains(searchQuery) || x.Country.Contains(searchQuery) || x.City.Contains(searchQuery))
                    .ToList();

                ViewBag.PublishersConf = _context.PublishersConferences
                    .Where(x => x.PublishingHouseName.Contains(searchQuery) || x.Country.Contains(searchQuery) || x.City.Contains(searchQuery))
                    .ToList();
            }
            else
            {
                ViewBag.BookFairs = _context.SharjahBookFairs.ToList();
                ViewBag.ChildFestivals = _context.SharjahChildFestivals.ToList();
                ViewBag.Distributors = _context.DistributorsConferences.ToList();
                ViewBag.NewYork = _context.NewYorkSessions.ToList();
                ViewBag.PublishersConf = _context.PublishersConferences.ToList();
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddRecord(string section, int exhibitionYear, int festivalYear, int conferenceYear, int sessionYear, string publishingHouseName, string country, string city, string whatsAppNumber, string email, string responsiblePerson, int bookCount, string specialization, string requiredSpace)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");

            if (section == "SharjahBookFairs")
            {
                _context.SharjahBookFairs.Add(new SharjahBookFair { 
                    ExhibitionYear = exhibitionYear, 
                    PublishingHouseName = publishingHouseName ?? "", 
                    Country = country ?? "", 
                    City = city ?? "", 
                    WhatsAppNumber = whatsAppNumber ?? "", 
                    Email = email ?? "", 
                    ResponsiblePerson = responsiblePerson ?? "", 
                    BookCount = bookCount, 
                    Specialization = specialization ?? "", 
                    RequiredSpace = requiredSpace ?? "" 
                });
            }
            else if (section == "SharjahChildFestivals")
            {
                _context.SharjahChildFestivals.Add(new SharjahChildFestival { 
                    FestivalYear = festivalYear, 
                    PublishingHouseName = publishingHouseName ?? "", 
                    Country = country ?? "", 
                    City = city ?? "", 
                    WhatsAppNumber = whatsAppNumber ?? "", 
                    Email = email ?? "", 
                    ResponsiblePerson = responsiblePerson ?? "", 
                    BookCount = bookCount, 
                    RequiredSpace = requiredSpace ?? "" 
                });
            }
            else if (section == "DistributorsConferences")
            {
                _context.DistributorsConferences.Add(new DistributorsConference { 
                    ConferenceYear = conferenceYear, 
                    PublishingHouseName = publishingHouseName ?? "", 
                    Country = country ?? "", 
                    City = city ?? "", 
                    WhatsAppNumber = whatsAppNumber ?? "", 
                    Email = email ?? "", 
                    ResponsiblePerson = responsiblePerson ?? "" 
                });
            }
            else if (section == "NewYorkSessions")
            {
                _context.NewYorkSessions.Add(new NewYorkSession { 
                    SessionYear = sessionYear, 
                    PublishingHouseName = publishingHouseName ?? "", 
                    Country = country ?? "", 
                    City = city ?? "", 
                    WhatsAppNumber = whatsAppNumber ?? "", 
                    Email = email ?? "", 
                    ResponsiblePerson = responsiblePerson ?? "" 
                });
            }
            else if (section == "PublishersConferences")
            {
                _context.PublishersConferences.Add(new PublishersConference { 
                    ConferenceYear = conferenceYear, 
                    PublishingHouseName = publishingHouseName ?? "", 
                    Country = country ?? "", 
                    City = city ?? "", 
                    WhatsAppNumber = whatsAppNumber ?? "", 
                    Email = email ?? "", 
                    ResponsiblePerson = responsiblePerson ?? "" 
                });
            }
            
            _context.SaveChanges();

            TempData["SuccessMessage"] = "تم حفظ البيانات بنجاح!";
            return RedirectToAction("Index");
        }
    }
}