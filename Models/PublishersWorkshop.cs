using System.ComponentModel.DataAnnotations;

namespace SharjahEventsWeb.Models
{
    public class PublishersWorkshop
    {
        [Key]
        public int Id { get; set; }
        public int WorkshopYear { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string WhatsAppNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}