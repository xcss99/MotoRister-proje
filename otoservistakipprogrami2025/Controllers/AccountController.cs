using Microsoft.AspNetCore.Mvc;
using otoservistakipprogrami2025.DAL;
using otoservistakipprogrami2025.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace otoservistakipprogrami2025.Controllers
{
    public class AccountController : Controller
    {
        private readonly OtoServisDbContext _context;

        public AccountController(OtoServisDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel? model)
        {
            Console.WriteLine($"Login POST çağrıldı - Email: {model?.Email}");

            if (model == null || !ModelState.IsValid)
            {
                if (model == null)
                {
                    Console.WriteLine("Model null geldi");
                    ViewBag.ErrorMessage = "Geçersiz istek!";
                }
                else
                {
                    Console.WriteLine("ModelState geçersiz");
                    ViewBag.ErrorMessage = "Lütfen tüm alanları doğru doldurunuz!";
                }
                return View(model);
            }

            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.IsActive);

                Console.WriteLine($"Kullanıcı sorgulandı: {(user != null ? "Bulundu" : "Bulunamadı")}");

                if (user != null && user.Password == model.Password) // (Hashleme kullanmıyorsan böyle bırak)
                {
                    Console.WriteLine("Giriş başarılı, Session başlatılıyor");

                    HttpContext.Session.SetString("UserId", user.UserId.ToString());
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserName", $"{user.Ad} {user.Soyad}");

                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    Console.WriteLine("Şifre hatalı veya kullanıcı yok");
                    ViewBag.ErrorMessage = "E-posta veya şifre hatalı!";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login hatası: {ex.Message}");
                ViewBag.ErrorMessage = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return View(model);
            }
        }
    }
}
