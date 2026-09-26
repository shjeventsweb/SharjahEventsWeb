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
                var allFairs = _context.SharjahBookFairs
                    .FromSqlRaw("SELECT * FROM \"SharjahBookFairs\"")
                    .ToList();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    ViewBag.BookFairs = allFairs
                        .Where(x => (x.PublishingHouseName != null && x.PublishingHouseName.Contains(searchQuery)) || 
                                    (x.Country != null && x.Country.Contains(searchQuery)) || 
                                    (x.City != null && x.City.Contains(searchQuery)))
                        .ToList();
                }
                else
                {
                    ViewBag.BookFairs = allFairs;
                }

                ViewBag.ChildFestivals = _context.SharjahChildFestivals.FromSqlRaw("SELECT * FROM \"SharjahChildFestivals\"").ToList();
                ViewBag.Distributors = _context.DistributorsConferences.FromSqlRaw("SELECT * FROM \"DistributorsConferences\"").ToList();
                ViewBag.NewYork = _context.NewYorkSessions.FromSqlRaw("SELECT * FROM \"NewYorkSessions\"").ToList();
                ViewBag.PublishersConf = _context.PublishersConferences.FromSqlRaw("SELECT * FROM \"PublishersConferences\"").ToList();
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
        public IActionResult AddRecord(string section)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");
            ViewBag.Section = section;
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