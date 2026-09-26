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

            var dbUser = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (dbUser != null)
            {
                HttpContext.Session.SetString("UserEmail", email);
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
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
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "الرجاء إدخال البريد الإلكتروني وكلمة المرور.");
                return View();
            }

            bool userExists = _context.Users.Any(u => u.Email == email);
            if (userExists)
            {
                ModelState.AddModelError("", "البريد الإلكتروني مستخدم مسبقاً.");
                return View();
            }

            _context.Users.Add(new UserAccount
            {
                Email = email,
                Password = password
            });
            _context.SaveChanges();

            TempData["SuccessMessage"] = "تم إنشاء الحساب بنجاح! يمكنك تسجيل الدخول الآن.";
            return RedirectToAction("Login");
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

            try
            {
                var bookFairsQuery = _context.SharjahBookFairs.AsQueryable();
                var childFestivalsQuery = _context.SharjahChildFestivals.AsQueryable();
                var distributorsQuery = _context.DistributorsConferences.AsQueryable();
                var newYorkQuery = _context.NewYorkSessions.AsQueryable();
                var publishersConfQuery = _context.PublishersConferences.AsQueryable();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    bookFairsQuery = bookFairsQuery.Where(x => 
                        (!string.IsNullOrEmpty(x.PublishingHouseName) && x.PublishingHouseName.Contains(searchQuery)) || 
                        (!string.IsNullOrEmpty(x.Country) && x.Country.Contains(searchQuery)) || 
                        (!string.IsNullOrEmpty(x.City) && x.City.Contains(searchQuery)));
                }

                ViewBag.BookFairs = bookFairsQuery.ToList();
                ViewBag.ChildFestivals = childFestivalsQuery.ToList();
                ViewBag.Distributors = distributorsQuery.ToList();
                ViewBag.NewYork = newYorkQuery.ToList();
                ViewBag.PublishersConf = publishersConfQuery.ToList();
            }
            catch (Exception)
            {
                ViewBag.BookFairs = new List<SharjahBookFair>();
                ViewBag.ChildFestivals = new List<SharjahChildFestival>();
                ViewBag.Distributors = new List<DistributorsConference>();
                ViewBag.NewYork = new List<NewYorkSession>();
                ViewBag.PublishersConf = new List<PublishersConference>();
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddRecord(string section)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");
            ViewBag.Section = section;
            return View();
        }

        [HttpPost]
        public IActionResult AddRecord(string section, int year, int exhibitionYear, int festivalYear, int conferenceYear, int sessionYear, string houseName, string publishingHouseName, string country, string city, string whatsapp, string whatsAppNumber, string email, string person, string responsiblePerson, int bookCount, string specialization, string requiredSpace)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");

            int finalYear = year != 0 ? year : (exhibitionYear != 0 ? exhibitionYear : (festivalYear != 0 ? festivalYear : (conferenceYear != 0 ? conferenceYear : sessionYear)));
            string finalHouse = !string.IsNullOrEmpty(houseName) ? houseName : (publishingHouseName ?? "");
            string finalPhone = !string.IsNullOrEmpty(whatsapp) ? whatsapp : (whatsAppNumber ?? "");
            string finalPerson = !string.IsNullOrEmpty(person) ? person : (responsiblePerson ?? "");

            if (section == "SharjahBookFairs")
            {
                _context.SharjahBookFairs.Add(new SharjahBookFair { 
                    ExhibitionYear = finalYear, 
                    PublishingHouseName = finalHouse, 
                    Country = country ?? "", 
                    City = city ?? "", 
                    WhatsAppNumber = finalPhone, 
                    Email = email ?? "", 
                    ResponsiblePerson = finalPerson, 
                    BookCount = bookCount, 
                    Specialization = specialization ?? "", 
                    RequiredSpace = requiredSpace ?? "" 
                });
            }
            else if (section == "SharjahChildFestivals")
            {
                _context.SharjahChildFestivals.Add(new SharjahChildFestival { 
                    FestivalYear = finalYear, 
                    PublishingHouseName = finalHouse, 
                    Country = country ?? "", 
                    City = city ?? "", 
                    WhatsAppNumber = finalPhone, 
                    Email = email ?? "", 
                    ResponsiblePerson = finalPerson, 
                    BookCount = bookCount, 
                    RequiredSpace = requiredSpace ?? "" 
                });
            }
            else if (section == "DistributorsConferences")
            {
                _context.DistributorsConferences.Add(new DistributorsConference { 
                    ConferenceYear = finalYear, 
                    PublishingHouseName = finalHouse, 
                    Country = country ?? "", 
                    City = city ?? "", 
                    WhatsAppNumber = finalPhone, 
                    Email = email ?? "", 
                    ResponsiblePerson = finalPerson 
                });
            }
            else if (section == "NewYorkSessions")
            {
                _context.NewYorkSessions.Add(new NewYorkSession { 
                    SessionYear = finalYear, 
                    PublishingHouseName = finalHouse, 
                    Country = country ?? "", 
                    City = city ?? "", 
                    WhatsAppNumber = finalPhone, 
                    Email = email ?? "", 
                    ResponsiblePerson = finalPerson 
                });
            }
            else if (section == "PublishersConferences")
            {
                _context.PublishersConferences.Add(new PublishersConference { 
                    ConferenceYear = finalYear, 
                    PublishingHouseName = finalHouse, 
                    Country = country ?? "", 
                    City = city ?? "", 
                    WhatsAppNumber = finalPhone, 
                    Email = email ?? "", 
                    ResponsiblePerson = finalPerson 
                });
            }
            
            _context.SaveChanges();

            TempData["SuccessMessage"] = "تم حفظ البيانات بنجاح!";
            return RedirectToAction("Index");
        }
    }
}