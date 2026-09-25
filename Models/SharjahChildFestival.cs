using System.ComponentModel.DataAnnotations;

namespace SharjahEventsWeb.Models
{
    public class SharjahChildFestival
    {
        [Key]
        public int Id { get; set; }
        public int FestivalYear { get; set; }
        public string PublishingHouseName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string WhatsAppNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ResponsiblePerson { get; set; } = string.Empty;
        public int BookCount { get; set; }
        public decimal RequiredSpace { get; set; }
    }
}