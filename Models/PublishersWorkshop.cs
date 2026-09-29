using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharjahEventsWeb.Models
{
    [Table("PublishersWorkshops")]
    public class PublishersWorkshop
    {
        [Key]
        public int Id { get; set; }
        public int WorkshopYear { get; set; }
        
        [Column("LecturerName")]
        public string? LecturerName { get; set; }

        public string? Country { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? Email { get; set; }

        // حقول افتراضية لكي لا تفشل قاعدة البيانات في حال تطلبها الجدول
        public string? City { get; set; } = "الشارقة";
        public string? ResponsiblePerson { get; set; } = "-";
        public int BookCount { get; set; } = 0;
        public string? Specialization { get; set; } = "عام";
    }
}