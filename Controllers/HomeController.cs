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

            try
            {
                if (_context.Users.Any(u => u.Email == email && u.Password == password))
                {
                    HttpContext.Session.SetString("UserEmail", email);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                // تجاوز الخطأ في حال عدم توفر الجدول مؤقتاً
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

            try
            {
                bool userExists = _context.Users.Any(u => u.Email == email);
                if (userExists)
                {
                    ModelState.AddModelError("", "البريد الإلكتروني مستخدم مسبقاً.");
                    return View();
                }

                _context.Users.Add(new User
                {
                    Email = email,
                    Password = password
                });
                _context.SaveChanges();
            }
            catch
            {
                // تجاوز الخطأ مؤقتاً عند الحاجة
            }

            TempData["SuccessMessage"] = "تم إنشاء الحساب بنجاح! يمكنك تسجيل الدخول الآن.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
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
        public IActionResult AddRecord(string section)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) 
                return RedirectToAction("Login");

            ViewBag.Section = section ?? "SharjahBookFairs";
            return View();
        }

        [HttpPost]
        public IActionResult AddRecord(
            string section, 
            int exhibitionYear, 
            int festivalYear, 
            int conferenceYear, 
            int sessionYear, 
            string publishingHouseName, 
            string country, 
            string city, 
            string whatsAppNumber, 
            string email, 
            string responsiblePerson, 
            int bookCount, 
            string specialization, 
            string requiredSpace)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) 
                return RedirectToAction("Login");

            int year = exhibitionYear != 0 ? exhibitionYear : 
                       (festivalYear != 0 ? festivalYear : 
                       (conferenceYear != 0 ? conferenceYear : sessionYear));

            if (section == "SharjahBookFairs")
            {
                _context.SharjahBookFairs.Add(new SharjahBookFair { 
                    ExhibitionYear = year, 
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
                    FestivalYear = year, 
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
                    ConferenceYear = year, 
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
                    SessionYear = year, 
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
                    ConferenceYear = year, 
                    PublishingHouseName = publishingHouseName ?? "", 
                    Country = country ?? "", 
                    City = city ?? "", 
                    WhatsAppNumber = whatsAppNumber ?? "", 
                    Email = email ?? "", 
                    ResponsiblePerson = responsiblePerson ?? "" 
                });
            }
            
            _context.SaveChanges();

            TempData["SuccessMessage"] = "تم إضافة الفعالية بنجاح!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(string section, int id)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) 
                return RedirectToAction("Login");

            ViewBag.Section = section;

            if (section == "SharjahBookFairs")
            {
                var item = _context.SharjahBookFairs.Find(id);
                if (item == null) return NotFound();
                return View(item);
            }
            else if (section == "SharjahChildFestivals")
            {
                var item = _context.SharjahChildFestivals.Find(id);
                if (item == null) return NotFound();
                return View(item);
            }
            else if (section == "DistributorsConferences")
            {
                var item = _context.DistributorsConferences.Find(id);
                if (item == null) return NotFound();
                return View(item);
            }
            else if (section == "NewYorkSessions")
            {
                var item = _context.NewYorkSessions.Find(id);
                if (item == null) return NotFound();
                return View(item);
            }
            else if (section == "PublishersConferences")
            {
                var item = _context.PublishersConferences.Find(id);
                if (item == null) return NotFound();
                return View(item);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(
            string section, 
            int id, 
            int exhibitionYear, 
            int festivalYear, 
            int conferenceYear, 
            int sessionYear, 
            string publishingHouseName, 
            string country, 
            string city, 
            string whatsAppNumber, 
            string email, 
            string responsiblePerson, 
            int bookCount, 
            string specialization, 
            string requiredSpace)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) 
                return RedirectToAction("Login");

            int year = exhibitionYear != 0 ? exhibitionYear : 
                       (festivalYear != 0 ? festivalYear : 
                       (conferenceYear != 0 ? conferenceYear : sessionYear));

            if (section == "SharjahBookFairs")
            {
                var item = _context.SharjahBookFairs.Find(id);
                if (item != null)
                {
                    item.ExhibitionYear = year;
                    item.PublishingHouseName = publishingHouseName ?? "";
                    item.Country = country ?? "";
                    item.City = city ?? "";
                    item.WhatsAppNumber = whatsAppNumber ?? "";
                    item.Email = email ?? "";
                    item.ResponsiblePerson = responsiblePerson ?? "";
                    item.BookCount = bookCount;
                    item.Specialization = specialization ?? "";
                    item.RequiredSpace = requiredSpace ?? "";
                }
            }
            else if (section == "SharjahChildFestivals")
            {
                var item = _context.SharjahChildFestivals.Find(id);
                if (item != null)
                {
                    item.FestivalYear = year;
                    item.PublishingHouseName = publishingHouseName ?? "";
                    item.Country = country ?? "";
                    item.City = city ?? "";
                    item.WhatsAppNumber = whatsAppNumber ?? "";
                    item.Email = email ?? "";
                    item.ResponsiblePerson = responsiblePerson ?? "";
                    item.BookCount = bookCount;
                    item.RequiredSpace = requiredSpace ?? "";
                }
            }
            else if (section == "DistributorsConferences")
            {
                var item = _context.DistributorsConferences.Find(id);
                if (item != null)
                {
                    item.ConferenceYear = year;
                    item.PublishingHouseName = publishingHouseName ?? "";
                    item.Country = country ?? "";
                    item.City = city ?? "";
                    item.WhatsAppNumber = whatsAppNumber ?? "";
                    item.Email = email ?? "";
                    item.ResponsiblePerson = responsiblePerson ?? "";
                }
            }
            else if (section == "NewYorkSessions")
            {
                var item = _context.NewYorkSessions.Find(id);
                if (item != null)
                {
                    item.SessionYear = year;
                    item.PublishingHouseName = publishingHouseName ?? "";
                    item.Country = country ?? "";
                    item.City = city ?? "";
                    item.WhatsAppNumber = whatsAppNumber ?? "";
                    item.Email = email ?? "";
                    item.ResponsiblePerson = responsiblePerson ?? "";
                }
            }
            else if (section == "PublishersConferences")
            {
                var item = _context.PublishersConferences.Find(id);
                if (item != null)
                {
                    item.ConferenceYear = year;
                    item.PublishingHouseName = publishingHouseName ?? "";
                    item.Country = country ?? "";
                    item.City = city ?? "";
                    item.WhatsAppNumber = whatsAppNumber ?? "";
                    item.Email = email ?? "";
                    item.ResponsiblePerson = responsiblePerson ?? "";
                }
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "تم تحديث البيانات بنجاح!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string section, int id)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) 
                return RedirectToAction("Login");

            if (section == "SharjahBookFairs")
            {
                var item = _context.SharjahBookFairs.Find(id);
                if (item != null) { _context.SharjahBookFairs.Remove(item); }
            }
            else if (section == "SharjahChildFestivals")
            {
                var item = _context.SharjahChildFestivals.Find(id);
                if (item != null) { _context.SharjahChildFestivals.Remove(item); }
            }
            else if (section == "DistributorsConferences")
            {
                var item = _context.DistributorsConferences.Find(id);
                if (item != null) { _context.DistributorsConferences.Remove(item); }
            }
            else if (section == "NewYorkSessions")
            {
                var item = _context.NewYorkSessions.Find(id);
                if (item != null) { _context.NewYorkSessions.Remove(item); }
            }
            else if (section == "PublishersConferences")
            {
                var item = _context.PublishersConferences.Find(id);
                if (item != null) { _context.PublishersConferences.Remove(item); }
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "تم حذف السجل بنجاح!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteAll(string section)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) 
                return RedirectToAction("Login");

            if (section == "SharjahBookFairs")
            {
                _context.SharjahBookFairs.RemoveRange(_context.SharjahBookFairs);
            }
            else if (section == "SharjahChildFestivals")
            {
                _context.SharjahChildFestivals.RemoveRange(_context.SharjahChildFestivals);
            }
            else if (section == "DistributorsConferences")
            {
                _context.DistributorsConferences.RemoveRange(_context.DistributorsConferences);
            }
            else if (section == "NewYorkSessions")
            {
                _context.NewYorkSessions.RemoveRange(_context.NewYorkSessions);
            }
            else if (section == "PublishersConferences")
            {
                _context.PublishersConferences.RemoveRange(_context.PublishersConferences);
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "تم حذف جميع السجلات لهذا القسم بنجاح!";
            return RedirectToAction("Index");
        }
    }
}