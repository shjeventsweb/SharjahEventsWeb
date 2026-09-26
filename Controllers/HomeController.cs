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
            if (HttpContext.Session.GetString("UserEmail") != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.FullName ?? "مستخدم");
                return RedirectToAction("Index");
            }

            ViewBag.Error = "البريد الإلكتروني أو كلمة المرور غير صحيحة";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Index(string section = "SharjahBookFairs")
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.CurrentSection = section;

            if (section == "SharjahBookFairs")
            {
                var data = await _context.SharjahBookFairs.ToListAsync();
                return View(data);
            }
            else if (section == "SharjahChildFestivals")
            {
                var data = await _context.SharjahChildFestivals.ToListAsync();
                return View(data);
            }
            else if (section == "PublishersWorkshops")
            {
                var data = await _context.PublishersWorkshops.ToListAsync();
                return View(data);
            }
            else if (section == "DistributorsWorkshops")
            {
                var data = await _context.DistributorsWorkshops.ToListAsync();
                return View(data);
            }

            return View(new List<SharjahBookFair>());
        }

        [HttpGet]
        public IActionResult AddRecord(string section = "SharjahBookFairs")
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Section = section;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(
            string section,
            int exhibitionYear,
            int festivalYear,
            int workshopYear,
            string publishingHouseName,
            string country,
            string city,
            string whatsAppNumber,
            string email,
            string responsiblePerson,
            int bookCount,
            string specialization,
            string requiredSpace,
            string lecturerName)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login");
            }

            if (section == "SharjahBookFairs")
            {
                var record = new SharjahBookFair
                {
                    ExhibitionYear = exhibitionYear > 0 ? exhibitionYear : 2026,
                    PublishingHouseName = publishingHouseName ?? "",
                    Country = country ?? "",
                    City = city ?? "",
                    WhatsAppNumber = whatsAppNumber ?? "",
                    Email = email ?? "",
                    ResponsiblePerson = responsiblePerson ?? "",
                    BookCount = bookCount,
                    Specialization = specialization ?? "",
                    RequiredSpace = requiredSpace ?? ""
                };
                _context.SharjahBookFairs.Add(record);
            }
            else if (section == "SharjahChildFestivals")
            {
                var record = new SharjahChildFestival
                {
                    FestivalYear = festivalYear > 0 ? festivalYear : 2026,
                    PublishingHouseName = publishingHouseName ?? "",
                    Country = country ?? "",
                    City = city ?? "",
                    WhatsAppNumber = whatsAppNumber ?? "",
                    Email = email ?? "",
                    ResponsiblePerson = responsiblePerson ?? "",
                    BookCount = bookCount,
                    RequiredSpace = requiredSpace ?? ""
                };
                _context.SharjahChildFestivals.Add(record);
            }
            else if (section == "PublishersWorkshops")
            {
                var record = new PublishersWorkshop
                {
                    WorkshopYear = workshopYear > 0 ? workshopYear : 2026,
                    PublishingHouseName = publishingHouseName ?? "",
                    Country = country ?? "",
                    City = city ?? "",
                    WhatsAppNumber = whatsAppNumber ?? "",
                    Email = email ?? "",
                    ResponsiblePerson = responsiblePerson ?? "",
                    BookCount = bookCount,
                    Specialization = specialization ?? "",
                    RequiredSpace = requiredSpace ?? "",
                    LecturerName = lecturerName ?? ""
                };
                _context.PublishersWorkshops.Add(record);
            }
            else if (section == "DistributorsWorkshops")
            {
                var record = new DistributorsWorkshop
                {
                    WorkshopYear = workshopYear > 0 ? workshopYear : 2026,
                    PublishingHouseName = publishingHouseName ?? "",
                    Country = country ?? "",
                    City = city ?? "",
                    WhatsAppNumber = whatsAppNumber ?? "",
                    Email = email ?? "",
                    ResponsiblePerson = responsiblePerson ?? ""
                };
                _context.DistributorsWorkshops.Add(record);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { section = section });
        }
    }
}