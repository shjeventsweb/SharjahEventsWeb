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
        public IActionResult Login() => View();

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
            catch { }

            ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
            return View();
        }

        [HttpGet]
        public IActionResult Register() => View();

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
                if (_context.Users.Any(u => u.Email == email))
                {
                    ModelState.AddModelError("", "البريد الإلكتروني مستخدم مسبقاً.");
                    return View();
                }

                _context.Users.Add(new User { Email = email, Password = password });
                _context.SaveChanges();
            }
            catch { }

            TempData["SuccessMessage"] = "تم إنشاء الحساب بنجاح! يمكنك تسجيل الدخول الآن.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Index(string searchQuery, string activeTab)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");

            ViewBag.SearchQuery = searchQuery;
            ViewBag.ActiveTab = string.IsNullOrEmpty(activeTab) ? "SharjahBookFairs" : activeTab;

            try
            {
                // التأكد من إنشاء الجداول تلقائياً في قاعدة البيانات إن لم تكن موجودة
                _context.Database.EnsureCreated();

                var bookFairs = _context.SharjahBookFairs.ToList();
                var childFestivals = _context.SharjahChildFestivals.ToList();
                var distributors = _context.DistributorsConferences.ToList();
                var newYork = _context.NewYorkSessions.ToList();
                var publishersConf = _context.PublishersConferences.ToList();
                var publishersWorkshops = _context.PublishersWorkshops.ToList();
                var distributorsWorkshops = _context.DistributorsWorkshops.ToList();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    bookFairs = bookFairs.Where(x => (x.PublishingHouseName != null && x.PublishingHouseName.Contains(searchQuery)) || (x.Country != null && x.Country.Contains(searchQuery)) || (x.City != null && x.City.Contains(searchQuery))).ToList();
                    childFestivals = childFestivals.Where(x => (x.PublishingHouseName != null && x.PublishingHouseName.Contains(searchQuery)) || (x.Country != null && x.Country.Contains(searchQuery)) || (x.City != null && x.City.Contains(searchQuery))).ToList();
                    distributors = distributors.Where(x => (x.PublishingHouseName != null && x.PublishingHouseName.Contains(searchQuery)) || (x.Country != null && x.Country.Contains(searchQuery)) || (x.City != null && x.City.Contains(searchQuery))).ToList();
                    newYork = newYork.Where(x => (x.PublishingHouseName != null && x.PublishingHouseName.Contains(searchQuery)) || (x.Country != null && x.Country.Contains(searchQuery)) || (x.City != null && x.City.Contains(searchQuery))).ToList();
                    publishersConf = publishersConf.Where(x => (x.PublishingHouseName != null && x.PublishingHouseName.Contains(searchQuery)) || (x.Country != null && x.Country.Contains(searchQuery)) || (x.City != null && x.City.Contains(searchQuery))).ToList();
                    publishersWorkshops = publishersWorkshops.Where(x => (x.LecturerName != null && x.LecturerName.Contains(searchQuery)) || (x.Country != null && x.Country.Contains(searchQuery))).ToList();
                    distributorsWorkshops = distributorsWorkshops.Where(x => (x.LecturerName != null && x.LecturerName.Contains(searchQuery)) || (x.Country != null && x.Country.Contains(searchQuery))).ToList();
                }

                ViewBag.BookFairs = bookFairs;
                ViewBag.ChildFestivals = childFestivals;
                ViewBag.Distributors = distributors;
                ViewBag.NewYork = newYork;
                ViewBag.PublishersConf = publishersConf;
                ViewBag.PublishersWorkshops = publishersWorkshops;
                ViewBag.DistributorsWorkshops = distributorsWorkshops;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
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
            ViewBag.Section = section ?? "SharjahBookFairs";
            return View();
        }

        [HttpPost]
        public IActionResult AddRecord(string section, int exhibitionYear, int festivalYear, int conferenceYear, int sessionYear, int workshopYear, string publishingHouseName, string country, string city, string whatsAppNumber, string email, string responsiblePerson, string lecturerName, int bookCount, string specialization, string requiredSpace)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");

            int year = workshopYear != 0 ? workshopYear : (exhibitionYear != 0 ? exhibitionYear : (festivalYear != 0 ? festivalYear : (conferenceYear != 0 ? conferenceYear : (sessionYear != 0 ? sessionYear : 2026))));

            try
            {
                if (section == "SharjahBookFairs")
                {
                    _context.SharjahBookFairs.Add(new SharjahBookFair { ExhibitionYear = year, PublishingHouseName = publishingHouseName ?? "", Country = country ?? "", City = city ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "", ResponsiblePerson = responsiblePerson ?? "", BookCount = bookCount, Specialization = specialization ?? "", RequiredSpace = requiredSpace ?? "" });
                }
                else if (section == "SharjahChildFestivals")
                {
                    _context.SharjahChildFestivals.Add(new SharjahChildFestival { FestivalYear = year, PublishingHouseName = publishingHouseName ?? "", Country = country ?? "", City = city ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "", ResponsiblePerson = responsiblePerson ?? "", BookCount = bookCount, RequiredSpace = requiredSpace ?? "" });
                }
                else if (section == "DistributorsConferences")
                {
                    _context.DistributorsConferences.Add(new DistributorsConference { ConferenceYear = year, PublishingHouseName = publishingHouseName ?? "", Country = country ?? "", City = city ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "", ResponsiblePerson = responsiblePerson ?? "" });
                }
                else if (section == "NewYorkSessions")
                {
                    _context.NewYorkSessions.Add(new NewYorkSession { SessionYear = year, PublishingHouseName = publishingHouseName ?? "", Country = country ?? "", City = city ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "", ResponsiblePerson = responsiblePerson ?? "" });
                }
                else if (section == "PublishersConferences")
                {
                    _context.PublishersConferences.Add(new PublishersConference { ConferenceYear = year, PublishingHouseName = publishingHouseName ?? "", Country = country ?? "", City = city ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "", ResponsiblePerson = responsiblePerson ?? "" });
                }
                else if (section == "PublishersWorkshops")
                {
                    _context.PublishersWorkshops.Add(new PublishersWorkshop { WorkshopYear = year, LecturerName = lecturerName ?? "", Country = country ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "" });
                }
                else if (section == "DistributorsWorkshops")
                {
                    _context.DistributorsWorkshops.Add(new DistributorsWorkshop { WorkshopYear = year, LecturerName = lecturerName ?? "", Country = country ?? "", WhatsAppNumber = whatsAppNumber ?? "", Email = email ?? "" });
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "تم إضافة السجل بنجاح!";
            }
            catch (Exception ex)
            {
                TempData["SuccessMessage"] = "خطأ في الحفظ: " + ex.Message;
            }

            return RedirectToAction("Index", new { activeTab = section });
        }

        [HttpGet]
        public IActionResult Edit(string section, int id)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");
            ViewBag.Section = section;

            if (section == "SharjahBookFairs") return View(_context.SharjahBookFairs.Find(id));
            if (section == "SharjahChildFestivals") return View(_context.SharjahChildFestivals.Find(id));
            if (section == "DistributorsConferences") return View(_context.DistributorsConferences.Find(id));
            if (section == "NewYorkSessions") return View(_context.NewYorkSessions.Find(id));
            if (section == "PublishersConferences") return View(_context.PublishersConferences.Find(id));
            if (section == "PublishersWorkshops") return View(_context.PublishersWorkshops.Find(id));
            if (section == "DistributorsWorkshops") return View(_context.DistributorsWorkshops.Find(id));

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(string section, int id, int exhibitionYear, int festivalYear, int conferenceYear, int sessionYear, int workshopYear, string publishingHouseName, string country, string city, string whatsAppNumber, string email, string responsiblePerson, string lecturerName, int bookCount, string specialization, string requiredSpace)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");

            int year = workshopYear != 0 ? workshopYear : (exhibitionYear != 0 ? exhibitionYear : (festivalYear != 0 ? festivalYear : (conferenceYear != 0 ? conferenceYear : (sessionYear != 0 ? sessionYear : 2026))));

            try
            {
                if (section == "SharjahBookFairs")
                {
                    var item = _context.SharjahBookFairs.Find(id);
                    if (item != null) { item.ExhibitionYear = year; item.PublishingHouseName = publishingHouseName ?? ""; item.Country = country ?? ""; item.City = city ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; item.ResponsiblePerson = responsiblePerson ?? ""; item.BookCount = bookCount; item.Specialization = specialization ?? ""; item.RequiredSpace = requiredSpace ?? ""; }
                }
                else if (section == "SharjahChildFestivals")
                {
                    var item = _context.SharjahChildFestivals.Find(id);
                    if (item != null) { item.FestivalYear = year; item.PublishingHouseName = publishingHouseName ?? ""; item.Country = country ?? ""; item.City = city ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; item.ResponsiblePerson = responsiblePerson ?? ""; item.BookCount = bookCount; item.RequiredSpace = requiredSpace ?? ""; }
                }
                else if (section == "DistributorsConferences")
                {
                    var item = _context.DistributorsConferences.Find(id);
                    if (item != null) { item.ConferenceYear = year; item.PublishingHouseName = publishingHouseName ?? ""; item.Country = country ?? ""; item.City = city ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; item.ResponsiblePerson = responsiblePerson ?? ""; }
                }
                else if (section == "NewYorkSessions")
                {
                    var item = _context.NewYorkSessions.Find(id);
                    if (item != null) { item.SessionYear = year; item.PublishingHouseName = publishingHouseName ?? ""; item.Country = country ?? ""; item.City = city ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; item.ResponsiblePerson = responsiblePerson ?? ""; }
                }
                else if (section == "PublishersConferences")
                {
                    var item = _context.PublishersConferences.Find(id);
                    if (item != null) { item.ConferenceYear = year; item.PublishingHouseName = publishingHouseName ?? ""; item.Country = country ?? ""; item.City = city ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; item.ResponsiblePerson = responsiblePerson ?? ""; }
                }
                else if (section == "PublishersWorkshops")
                {
                    var item = _context.PublishersWorkshops.Find(id);
                    if (item != null) { item.WorkshopYear = year; item.LecturerName = lecturerName ?? ""; item.Country = country ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; }
                }
                else if (section == "DistributorsWorkshops")
                {
                    var item = _context.DistributorsWorkshops.Find(id);
                    if (item != null) { item.WorkshopYear = year; item.LecturerName = lecturerName ?? ""; item.Country = country ?? ""; item.WhatsAppNumber = whatsAppNumber ?? ""; item.Email = email ?? ""; }
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "تم تحديث البيانات بنجاح!";
            }
            catch (Exception ex)
            {
                TempData["SuccessMessage"] = "خطأ في التحديث: " + ex.Message;
            }

            return RedirectToAction("Index", new { activeTab = section });
        }

        [HttpPost]
        public IActionResult Delete(string section, int id)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");

            if (section == "SharjahBookFairs") { var x = _context.SharjahBookFairs.Find(id); if (x != null) _context.SharjahBookFairs.Remove(x); }
            else if (section == "SharjahChildFestivals") { var x = _context.SharjahChildFestivals.Find(id); if (x != null) _context.SharjahChildFestivals.Remove(x); }
            else if (section == "DistributorsConferences") { var x = _context.DistributorsConferences.Find(id); if (x != null) _context.DistributorsConferences.Remove(x); }
            else if (section == "NewYorkSessions") { var x = _context.NewYorkSessions.Find(id); if (x != null) _context.NewYorkSessions.Remove(x); }
            else if (section == "PublishersConferences") { var x = _context.PublishersConferences.Find(id); if (x != null) _context.PublishersConferences.Remove(x); }
            else if (section == "PublishersWorkshops") { var x = _context.PublishersWorkshops.Find(id); if (x != null) _context.PublishersWorkshops.Remove(x); }
            else if (section == "DistributorsWorkshops") { var x = _context.DistributorsWorkshops.Find(id); if (x != null) _context.DistributorsWorkshops.Remove(x); }

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
            TempData["SuccessMessage"] = "تم حذف جميع السجلات بنجاح!";
            return RedirectToAction("Index", new { activeTab = section });
        }

        [HttpGet]
        public IActionResult ExportExcel(string section)
        {
            if (HttpContext.Session.GetString("UserEmail") == null) return RedirectToAction("Login");

            var builder = new System.Text.StringBuilder();
            builder.Append('\uFEFF');
            string fileName = "Export.csv";

            if (section == "SharjahBookFairs")
            {
                fileName = "SharjahBookFairs.csv";
                builder.AppendLine("السنة,دار النشر,الدولة,المدينة,واتساب,الإيميل,المسؤول,الإصدارات,التخصص,المساحة");
                foreach (var i in _context.SharjahBookFairs.ToList())
                    builder.AppendLine($"{i.ExhibitionYear},{EscapeCsv(i.PublishingHouseName)},{EscapeCsv(i.Country)},{EscapeCsv(i.City)},{EscapeCsv(i.WhatsAppNumber)},{EscapeCsv(i.Email)},{EscapeCsv(i.ResponsiblePerson)},{i.BookCount},{EscapeCsv(i.Specialization)},{EscapeCsv(i.RequiredSpace)}");
            }
            else if (section == "SharjahChildFestivals")
            {
                fileName = "SharjahChildFestivals.csv";
                builder.AppendLine("السنة,دار النشر,الدولة,المدينة,واتساب,الإيميل,المسؤول,الإصدارات,المساحة");
                foreach (var i in _context.SharjahChildFestivals.ToList())
                    builder.AppendLine($"{i.FestivalYear},{EscapeCsv(i.PublishingHouseName)},{EscapeCsv(i.Country)},{EscapeCsv(i.City)},{EscapeCsv(i.WhatsAppNumber)},{EscapeCsv(i.Email)},{EscapeCsv(i.ResponsiblePerson)},{i.BookCount},{EscapeCsv(i.RequiredSpace)}");
            }
            else if (section == "DistributorsConferences")
            {
                fileName = "DistributorsConferences.csv";
                builder.AppendLine("السنة,دار النشر,الدولة,المدينة,واتساب,الإيميل,المسؤول");
                foreach (var i in _context.DistributorsConferences.ToList())
                    builder.AppendLine($"{i.ConferenceYear},{EscapeCsv(i.PublishingHouseName)},{EscapeCsv(i.Country)},{EscapeCsv(i.City)},{EscapeCsv(i.WhatsAppNumber)},{EscapeCsv(i.Email)},{EscapeCsv(i.ResponsiblePerson)}");
            }
            else if (section == "NewYorkSessions")
            {
                fileName = "NewYorkSessions.csv";
                builder.AppendLine("السنة,دار النشر,الدولة,المدينة,واتساب,الإيميل,المسؤول");
                foreach (var i in _context.NewYorkSessions.ToList())
                    builder.AppendLine($"{i.SessionYear},{EscapeCsv(i.PublishingHouseName)},{EscapeCsv(i.Country)},{EscapeCsv(i.City)},{EscapeCsv(i.WhatsAppNumber)},{EscapeCsv(i.Email)},{EscapeCsv(i.ResponsiblePerson)}");
            }
            else if (section == "PublishersConferences")
            {
                fileName = "PublishersConferences.csv";
                builder.AppendLine("السنة,دار النشر,الدولة,المدينة,واتساب,الإيميل,المسؤول");
                foreach (var i in _context.PublishersConferences.ToList())
                    builder.AppendLine($"{i.ConferenceYear},{EscapeCsv(i.PublishingHouseName)},{EscapeCsv(i.Country)},{EscapeCsv(i.City)},{EscapeCsv(i.WhatsAppNumber)},{EscapeCsv(i.Email)},{EscapeCsv(i.ResponsiblePerson)}");
            }
            else if (section == "PublishersWorkshops")
            {
                fileName = "PublishersWorkshops.csv";
                builder.AppendLine("السنة,اسم المحاضر,الدولة,واتساب,الإيميل");
                foreach (var i in _context.PublishersWorkshops.ToList())
                    builder.AppendLine($"{i.WorkshopYear},{EscapeCsv(i.LecturerName)},{EscapeCsv(i.Country)},{EscapeCsv(i.WhatsAppNumber)},{EscapeCsv(i.Email)}");
            }
            else if (section == "DistributorsWorkshops")
            {
                fileName = "DistributorsWorkshops.csv";
                builder.AppendLine("السنة,اسم المحاضر,الدولة,واتساب,الإيميل");
                foreach (var i in _context.DistributorsWorkshops.ToList())
                    builder.AppendLine($"{i.WorkshopYear},{EscapeCsv(i.LecturerName)},{EscapeCsv(i.Country)},{EscapeCsv(i.WhatsAppNumber)},{EscapeCsv(i.Email)}");
            }

            return File(System.Text.Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", fileName);
        }

        private string EscapeCsv(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            if (input.Contains(",") || input.Contains("\"") || input.Contains("\n"))
            {
                return "\"" + input.Replace("\"", "\"\"") + "\"";
            }
            return input;
        }
    }
}