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