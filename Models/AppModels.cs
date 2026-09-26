using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserAccount> Users { get; set; }
    public DbSet<SharjahBookFair> SharjahBookFairs { get; set; }
    public DbSet<SharjahChildFestival> SharjahChildFestivals { get; set; }
    public DbSet<DistributorsConference> DistributorsConferences { get; set; }
    public DbSet<NewYorkSession> NewYorkSessions { get; set; }
    public DbSet<PublishersConference> PublishersConferences { get; set; }
}

[Table("Users")]
public class UserAccount
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

[Table("SharjahBookFairs")]
public class SharjahBookFair
{
    [Key]
    public int Id { get; set; }
    public int ExhibitionYear { get; set; }
    public string PublishingHouseName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string WhatsAppNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ResponsiblePerson { get; set; } = string.Empty;
    public int BookCount { get; set; }
    public string Specialization { get; set; } = string.Empty;
    public string RequiredSpace { get; set; } = string.Empty;
}

[Table("SharjahChildFestivals")]
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
    public string RequiredSpace { get; set; } = string.Empty;
}

[Table("DistributorsConferences")]
public class DistributorsConference
{
    [Key]
    public int Id { get; set; }
    public int ConferenceYear { get; set; }
    public string PublishingHouseName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string WhatsAppNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ResponsiblePerson { get; set; } = string.Empty;
}

[Table("NewYorkSessions")]
public class NewYorkSession
{
    [Key]
    public int Id { get; set; }
    public int SessionYear { get; set; }
    public string PublishingHouseName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string WhatsAppNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ResponsiblePerson { get; set; } = string.Empty;
}

[Table("PublishersConferences")]
public class PublishersConference
{
    [Key]
    public int Id { get; set; }
    public int ConferenceYear { get; set; }
    public string PublishingHouseName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string WhatsAppNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ResponsiblePerson { get; set; } = string.Empty;
}