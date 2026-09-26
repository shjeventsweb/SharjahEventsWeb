using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using System.Data;
using Microsoft.EntityFrameworkCore;
using SharjahEventsWeb.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SharjahEventsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            // جلب كافة السجلات من الجداول لتظهر في الواجهة
            ViewBag.SharjahBookFairs = _context.SharjahBookFairs.ToList();
            ViewBag.SharjahChildFestivals = _context.SharjahChildFestivals.ToList();
            ViewBag.DistributorsConferences = _context.DistributorsConferences.ToList();
            ViewBag.NewYorkSessions = _context.NewYorkSessions.ToList();
            ViewBag.PublishersConferences = _context.PublishersConferences.ToList();

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
                HttpContext.Session.SetString("UserEmail", user.Email);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "البريد الإلكتروني أو كلمة السر غير صحيحة.");
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string email, string password)
        {
            if (_context.Users.Any(u => u.Email == email))
            {
                ModelState.AddModelError("", "هذا البريد الإلكتروني مسجل مسبقاً.");
                return View();
            }

            _context.Users.Add(new UserAccount { Email = email, Password = password });
            _context.SaveChanges();

            TempData["SuccessMessage"] = "تم إنشاء الحساب بنجاح! يمكنك تسجيل الدخول الآن.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AddRecord(string section)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");
            ViewBag.Section = section;
            return View();
        }

        [HttpPost]
        public IActionResult AddRecord(string section, int year, string houseName, string country, string city, string whatsapp, string email, string person, int bookCount, string specialization, string requiredSpace)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");

            if (section == "SharjahBookFairs")
            {
                _context.SharjahBookFairs.Add(new SharjahBookFair 
                { 
                    ExhibitionYear = year, 
                    PublishingHouseName = houseName, 
                    Country = country, 
                    City = city, 
                    WhatsAppNumber = whatsapp, 
                    Email = email, 
                    ResponsiblePerson = person, 
                    BookCount = bookCount, 
                    Specialization = specialization ?? "", 
                    RequiredSpace = requiredSpace ?? "" 
                });
            }
            else if (section == "SharjahChildFestivals")
            {
                _context.SharjahChildFestivals.Add(new SharjahChildFestival 
                { 
                    FestivalYear = year, 
                    PublishingHouseName = houseName, 
                    Country = country, 
                    City = city, 
                    WhatsAppNumber = whatsapp, 
                    Email = email, 
                    ResponsiblePerson = person, 
                    BookCount = bookCount, 
                    RequiredSpace = requiredSpace ?? "" 
                });
            }
            else if (section == "DistributorsConferences")
            {
                _context.DistributorsConferences.Add(new DistributorsConference 
                { 
                    ConferenceYear = year, 
                    PublishingHouseName = houseName, 
                    Country = country, 
                    City = city, 
                    WhatsAppNumber = whatsapp, 
                    Email = email, 
                    ResponsiblePerson = person 
                });
            }
            else if (section == "NewYorkSessions")
            {
                _context.NewYorkSessions.Add(new NewYorkSession 
                { 
                    SessionYear = year, 
                    PublishingHouseName = houseName, 
                    Country = country, 
                    City = city, 
                    WhatsAppNumber = whatsapp, 
                    Email = email, 
                    ResponsiblePerson = person 
                });
            }
            else if (section == "PublishersConferences")
            {
                _context.PublishersConferences.Add(new PublishersConference 
                { 
                    ConferenceYear = year, 
                    PublishingHouseName = houseName, 
                    Country = country, 
                    City = city, 
                    WhatsAppNumber = whatsapp, 
                    Email = email, 
                    ResponsiblePerson = person 
                });
            }
            
            _context.SaveChanges();

            TempData["SuccessMessage"] = "تم حفظ البيانات بنجاح!";
            return RedirectToAction("Index");
        }
    }
}