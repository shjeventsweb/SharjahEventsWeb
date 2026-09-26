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

            _context.Users.Add(new User
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

        public IActionResult Index(string searchQuery, string activeTab)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.SearchQuery = searchQuery;
            ViewBag.ActiveTab = string.IsNullOrEmpty(activeTab) ? "SharjahBookFairs" : activeTab;

            try
            {
                ViewBag.BookFairs = _context.SharjahBookFairs.ToList();
                ViewBag.ChildFestivals = _context.SharjahChildFestivals.ToList();
                ViewBag.Distributors = _context.DistributorsConferences.ToList();
                ViewBag.NewYork = _context.NewYorkSessions.ToList();
                ViewBag.PublishersConf = _context.PublishersConferences.ToList();
                ViewBag.PublishersWorkshops = _context.PublishersWorkshops.ToList();
                ViewBag.DistributorsWorkshops = _context.DistributorsWorkshops.ToList();
            }
            catch (Exception)
            {
                ViewBag.BookFairs = new List<SharjahBookFair>();
                ViewBag.ChildFestivals = new List<SharjahChildFestival>();
                ViewBag.Distributors = new List<DistributorsConference>();
                ViewBag.NewYork = new List<NewYorkSession>();
                ViewBag.PublishersConf = new List<PublishersConference>();
                ViewBag.PublishersWorkshops = new List<PublishersWorkshop>();
                ViewBag.DistributorsWorkshops = new List<DistributorsWorkshop>();
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddRecord(string section)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");
            ViewBag.Section = string.IsNullOrEmpty(section) ? "SharjahBookFairs" : section;
            return View();
        }

        [HttpPost]
        public IActionResult AddRecord(string section, int workshopYear, string publishingHouseName, string country, string city, string whatsAppNumber, string email, string responsiblePerson, int bookCount, string specialization, string requiredSpace, string lecturerName)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");
            int finalYear = workshopYear == 0 ? 2026 : workshopYear;

            if (section == "SharjahBookFairs")
            {
                _context.SharjahBookFairs.Add(new SharjahBookFair { ExhibitionYear = finalYear, PublishingHouseName = publishingHouseName ?? "", Country = country ?? "", City = city ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "", ResponsiblePerson = responsiblePerson ?? "", BookCount = bookCount, Specialization = specialization ?? "", RequiredSpace = requiredSpace ?? "" });
            }
            else if (section == "SharjahChildFestivals")
            {
                _context.SharjahChildFestivals.Add(new SharjahChildFestival { FestivalYear = finalYear, PublishingHouseName = publishingHouseName ?? "", Country = country ?? "", City = city ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "", ResponsiblePerson = responsiblePerson ?? "", BookCount = bookCount, RequiredSpace = requiredSpace ?? "" });
            }
            else if (section == "DistributorsConferences")
            {
                _context.DistributorsConferences.Add(new DistributorsConference { ConferenceYear = finalYear, PublishingHouseName = publishingHouseName ?? "", Country = country ?? "", City = city ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "", ResponsiblePerson = responsiblePerson ?? "" });
            }
            else if (section == "NewYorkSessions")
            {
                _context.NewYorkSessions.Add(new NewYorkSession { SessionYear = finalYear, PublishingHouseName = publishingHouseName ?? "", Country = country ?? "", City = city ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "", ResponsiblePerson = responsiblePerson ?? "" });
            }
            else if (section == "PublishersConferences")
            {
                _context.PublishersConferences.Add(new PublishersConference { ConferenceYear = finalYear, PublishingHouseName = publishingHouseName ?? "", Country = country ?? "", City = city ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "", ResponsiblePerson = responsiblePerson ?? "" });
            }
            else if (section == "PublishersWorkshops")
            {
                _context.PublishersWorkshops.Add(new PublishersWorkshop { WorkshopYear = finalYear, LecturerName = lecturerName ?? "", Country = country ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "" });
            }
            else if (section == "DistributorsWorkshops")
            {
                _context.DistributorsWorkshops.Add(new DistributorsWorkshop { WorkshopYear = finalYear, LecturerName = lecturerName ?? "", Country = country ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "" });
            }
            
            _context.SaveChanges();
            TempData["SuccessMessage"] = "تم حفظ البيانات بنجاح!";
            return RedirectToAction("Index", new { activeTab = section });
        }

        [HttpGet]
        public IActionResult Edit(string section, int id)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");
            ViewBag.Section = section;
            ViewBag.Id = id;

            object? item = section switch
            {
                "SharjahBookFairs" => _context.SharjahBookFairs.Find(id),
                "SharjahChildFestivals" => _context.SharjahChildFestivals.Find(id),
                "DistributorsConferences" => _context.DistributorsConferences.Find(id),
                "NewYorkSessions" => _context.NewYorkSessions.Find(id),
                "PublishersConferences" => _context.PublishersConferences.Find(id),
                "PublishersWorkshops" => _context.PublishersWorkshops.Find(id),
                "DistributorsWorkshops" => _context.DistributorsWorkshops.Find(id),
                _ => null
            };

            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(string section, int id, int workshopYear, string publishingHouseName, string country, string city, string whatsAppNumber, string email, string responsiblePerson, string lecturerName)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");
            int finalYear = workshopYear == 0 ? 2026 : workshopYear;

            if (section == "SharjahBookFairs")
            {
                var item = _context.SharjahBookFairs.Find(id);
                if (item != null) { item.ExhibitionYear = finalYear; item.PublishingHouseName = publishingHouseName ?? ""; item.Country = country ?? ""; item.City = city ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; item.ResponsiblePerson = responsiblePerson ?? ""; }
            }
            else if (section == "SharjahChildFestivals")
            {
                var item = _context.SharjahChildFestivals.Find(id);
                if (item != null) { item.FestivalYear = finalYear; item.PublishingHouseName = publishingHouseName ?? ""; item.Country = country ?? ""; item.City = city ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; item.ResponsiblePerson = responsiblePerson ?? ""; }
            }
            else if (section == "DistributorsConferences")
            {
                var item = _context.DistributorsConferences.Find(id);
                if (item != null) { item.ConferenceYear = finalYear; item.PublishingHouseName = publishingHouseName ?? ""; item.Country = country ?? ""; item.City = city ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; item.ResponsiblePerson = responsiblePerson ?? ""; }
            }
            else if (section == "NewYorkSessions")
            {
                var item = _context.NewYorkSessions.Find(id);
                if (item != null) { item.SessionYear = finalYear; item.PublishingHouseName = publishingHouseName ?? ""; item.Country = country ?? ""; item.City = city ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; item.ResponsiblePerson = responsiblePerson ?? ""; }
            }
            else if (section == "PublishersConferences")
            {
                var item = _context.PublishersConferences.Find(id);
                if (item != null) { item.ConferenceYear = finalYear; item.PublishingHouseName = publishingHouseName ?? ""; item.Country = country ?? ""; item.City = city ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; item.ResponsiblePerson = responsiblePerson ?? ""; }
            }
            else if (section == "PublishersWorkshops")
            {
                var item = _context.PublishersWorkshops.Find(id);
                if (item != null) { item.WorkshopYear = finalYear; item.LecturerName = lecturerName ?? ""; item.Country = country ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; }
            }
            else if (section == "DistributorsWorkshops")
            {
                var item = _context.DistributorsWorkshops.Find(id);
                if (item != null) { item.WorkshopYear = finalYear; item.LecturerName = lecturerName ?? ""; item.Country = country ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; }
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "تم تحديث السجل بنجاح!";
            return RedirectToAction("Index", new { activeTab = section });
        }

        [HttpPost]
        public IActionResult Delete(string section, int id)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");

            if (section == "SharjahBookFairs") { var item = _context.SharjahBookFairs.Find(id); if (item != null) _context.SharjahBookFairs.Remove(item); }
            else if (section == "SharjahChildFestivals") { var item = _context.SharjahChildFestivals.Find(id); if (item != null) _context.SharjahChildFestivals.Remove(item); }
            else if (section == "DistributorsConferences") { var item = _context.DistributorsConferences.Find(id); if (item != null) _context.DistributorsConferences.Remove(item); }
            else if (section == "NewYorkSessions") { var item = _context.NewYorkSessions.Find(id); if (item != null) _context.NewYorkSessions.Remove(item); }
            else if (section == "PublishersConferences") { var item = _context.PublishersConferences.Find(id); if (item != null) _context.PublishersConferences.Remove(item); }
            else if (section == "PublishersWorkshops") { var item = _context.PublishersWorkshops.Find(id); if (item != null) _context.PublishersWorkshops.Remove(item); }
            else if (section == "DistributorsWorkshops") { var item = _context.DistributorsWorkshops.Find(id); if (item != null) _context.DistributorsWorkshops.Remove(item); }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "تم حذف السجل بنجاح!";
            return RedirectToAction("Index", new { activeTab = section });
        }

        [HttpPost]
        public IActionResult DeleteAll(string section)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");

            if (section == "SharjahBookFairs") _context.SharjahBookFairs.RemoveRange(_context.SharjahBookFairs);
            else if (section == "SharjahChildFestivals") _context.SharjahChildFestivals.RemoveRange(_context.SharjahChildFestivals);
            else if (section == "DistributorsConferences") _context.DistributorsConferences.RemoveRange(_context.DistributorsConferences);
            else if (section == "NewYorkSessions") _context.NewYorkSessions.RemoveRange(_context.NewYorkSessions);
            else if (section == "PublishersConferences") _context.PublishersConferences.RemoveRange(_context.PublishersConferences);
            else if (section == "PublishersWorkshops") _context.PublishersWorkshops.RemoveRange(_context.PublishersWorkshops);
            else if (section == "DistributorsWorkshops") _context.DistributorsWorkshops.RemoveRange(_context.DistributorsWorkshops);

            _context.SaveChanges();
            TempData["SuccessMessage"] = "تم حذف جميع السجلات من القسم بنجاح!";
            return RedirectToAction("Index", new { activeTab = section });
        }
    }
}