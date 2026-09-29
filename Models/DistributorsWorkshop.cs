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

        [Column("LecturerName")]
        public string? LecturerName { get; set; }

        [Column("PublishingHouseName")]
        public string? PublishingHouseName { get; set; }

        public string? Country { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; } = "الشارقة";
        public string? ResponsiblePerson { get; set; } = "-";
        public int BookCount { get; set; } = 0;
        public string? Specialization { get; set; } = "عام";
    }
}