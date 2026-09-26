using Microsoft.EntityFrameworkCore;

namespace SharjahEventsWeb.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<SharjahBookFair> SharjahBookFairs { get; set; }
        public DbSet<SharjahChildFestival> SharjahChildFestivals { get; set; }
        public DbSet<DistributorsConference> DistributorsConferences { get; set; }
        public DbSet<NewYorkSession> NewYorkSessions { get; set; }
        public DbSet<PublishersConference> PublishersConferences { get; set; }
        public DbSet<PublishersWorkshop> PublishersWorkshops { get; set; }
        public DbSet<DistributorsWorkshop> DistributorsWorkshops { get; set; }
    }
}