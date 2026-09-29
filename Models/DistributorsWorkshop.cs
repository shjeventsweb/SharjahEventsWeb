using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharjahEventsWeb.Models
{
    [Table("DistributorsWorkshops")]
    public class DistributorsWorkshop
    {
        [Key]
        public int Id { get; set; }
        public int WorkshopYear { get; set; }
        public string? LecturerName { get; set; }
        public string? Country { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? Email { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}