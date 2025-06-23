using Microsoft.EntityFrameworkCore;
using otoservistakipprogrami2025.DAL;
using otoservistakipprogrami2025.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<OtoServisDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Azure için ZORUNLU
    options.Cookie.SameSite = SameSiteMode.Lax; // Azure için ZORUNLU
    options.Cookie.Name = "SessionCookie"; // Ýsteðe baðlý
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Sadece bir kez
app.UseAuthentication(); // EKLE
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Test kullanýcýsý oluþtur
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<OtoServisDbContext>();
        context.Database.Migrate();
        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                Ad = "admin",
                Email = "admin@test.com",
                Password = "123456",
                KayitTarihi = DateTime.Now
            });
            context.SaveChanges();
        }
    }
}
catch (Exception ex)
{
    // Hata mesajýný dosyaya yaz (Azure'da göremeyebiliriz ama dursun)
    File.AppendAllText("log.txt", ex.ToString());

    // Alternatif olarak debug mesajý:
    Console.WriteLine("HATA: " + ex.Message);
}


app.Run(); // Sadece bir kez