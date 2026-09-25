using Microsoft.EntityFrameworkCore;
using SharjahEventsWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. تسجيل قاعدة بيانات Supabase (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. إضافة الخدمات الأساسية
builder.Services.AddControllersWithViews();

// 3. تفعيل الـ Session لتسجيل الدخول
builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

var app = builder.Build();

// 4. إنشاء الجداول تلقائياً في سحابة Supabase عند التشغيل الأول
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // محاولة اتصال خفيفة وسريعة بدون فحص الجداول الثقيل
    db.Database.CanConnect();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();