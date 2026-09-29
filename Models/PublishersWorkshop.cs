using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharjahEventsWeb.Models
{
    [Table("PublishersWorkshops")]
    public class PublishersWorkshop
    {
        [Key]
        public int Id { get; set; }

        [Column("WorkshopYear")]
        public int WorkshopYear { get; set; }

        [Column("LecturerName")]
        public string? LecturerName { get; set; }

        [Column("Country")]
        public string? Country { get; set; }

        [Column("WhatsAppNumber")]
        public string? WhatsAppNumber { get; set; }

        [Column("Email")]
        public string? Email { get; set; }
    }
}