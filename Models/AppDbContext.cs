using Microsoft.EntityFrameworkCore;
using SharjahEventsWeb.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<SharjahBookFair> SharjahBookFairs { get; set; }
    public DbSet<SharjahChildFestival> SharjahChildFestivals { get; set; }
    public DbSet<PublishersWorkshop> PublishersWorkshops { get; set; }
    public DbSet<DistributorsWorkshop> DistributorsWorkshops { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ربط صريح بأسماء الجداول في قاعدة البيانات لضمان عدم ضياع البيانات
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<SharjahBookFair>().ToTable("SharjahBookFairs");
        modelBuilder.Entity<SharjahChildFestival>().ToTable("SharjahChildFestivals");
        modelBuilder.Entity<PublishersWorkshop>().ToTable("PublishersWorkshops");
        modelBuilder.Entity<DistributorsWorkshop>().ToTable("DistributorsWorkshops");
    }
}