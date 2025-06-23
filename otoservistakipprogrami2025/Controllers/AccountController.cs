using Microsoft.AspNetCore.Mvc;
using otoservistakipprogrami2025.DAL;
using otoservistakipprogrami2025.Models;
using Microsoft.EntityFrameworkCore;

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
                }
                return View(model);
            }

            Console.WriteLine("ModelState geçti");
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.IsActive);

            Console.WriteLine($"User bulundu: {user != null}");

            if (user != null && user.Password == model.Password) // BCrypt.Verify yerine
            {
                Console.WriteLine("Şifre doğru, session kaydediliyor");
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("KullaniciAdi", user.Ad + " " + user.Soyad);
                Console.WriteLine("Login başarılı! Admin/Index'e yönlendiriliyor...");////
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                Console.WriteLine("Kullanıcı bulunamadı veya şifre yanlış");
                ViewBag.ErrorMessage = "E-posta veya şifre hatalı!";
            }

            return View(model);
        }
    }
}