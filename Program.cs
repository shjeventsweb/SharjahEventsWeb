using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// تسجيل قاعدة البيانات
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// إظهار الخطأ التقني الحقيقي على الشاشة لمعرفة سبب الـ 500 فوراً
app.UseDeveloperExceptionPage();

// حماية عملية التحقق من قاعدة البيانات بمنع الانهيار
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Warning: Could not connect to database on startup: {ex.Message}");
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