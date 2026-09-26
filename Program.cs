using Microsoft.EntityFrameworkCore;
using SharjahEventsWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Database configuration: SQLite for local development, PostgreSQL for production
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? Environment.GetEnvironmentVariable("DATABASE_URL") 
                       ?? builder.Configuration["ConnectionStrings:DefaultConnection"];

if (builder.Environment.IsDevelopment() || string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite("Data Source=events.db"));
}
else
{
    string npgsqlConnectionString;
    
    if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
    {
        var databaseUri = new Uri(connectionString);
        var userInfo = databaseUri.UserInfo.Split(':');
        var builder_conn = new Npgsql.NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port > 0 ? databaseUri.Port : 5432,
            Database = databaseUri.LocalPath.TrimStart('/'),
            Username = userInfo.Length > 0 ? userInfo[0] : "",
            Password = userInfo.Length > 1 ? userInfo[1] : "",
            SslMode = Npgsql.SslMode.Require,
            TrustServerCertificate = true
        };
        npgsqlConnectionString = builder_conn.ConnectionString;
    }
    else
    {
        npgsqlConnectionString = connectionString;
    }

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(npgsqlConnectionString));
}

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();