using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharjahEventsWeb.Models
{
    [Table("NewYorkSessions")]
    public class NewYorkSession
    {
        [Key]
        public int Id { get; set; }

        [Column("SessionYear")]
        public int SessionYear { get; set; }

        [Column("PublishingHouseName")]
        public string? PublishingHouseName { get; set; }

        [Column("Country")]
        public string? Country { get; set; }

        [Column("City")]
        public string? City { get; set; }

        [Column("WhatsAppNumber")]
        public string? WhatsAppNumber { get; set; }

        [Column("Email")]
        public string? Email { get; set; }

        [Column("ResponsiblePerson")]
        public string? ResponsiblePerson { get; set; }

        [Column("IsCompleted")]
        public bool IsCompleted { get; set; } = false;
    }
}