using System.ComponentModel.DataAnnotations;

namespace SharjahEventsWeb.Models
{
    public class NewYorkSession
    {
        [Key]
        public int Id { get; set; }
        public int SessionYear { get; set; }
        public string PublishingHouseName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string WhatsAppNumber { get; set; }
        public string Email { get; set; }
        public string ResponsiblePerson { get; set; }
    }
}