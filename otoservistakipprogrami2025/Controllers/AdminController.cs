using Microsoft.AspNetCore.Mvc;
using otoservistakipprogrami2025.Filters;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        // Session kontrolü - zaten [AdminAuthorize] filter'ı var, 
        // ama ekstra kontrol yapmak istersen:
        var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
        if (string.IsNullOrEmpty(kullaniciAdi))
        {
            // Login sayfasına yönlendir, Admin'e değil!
            return RedirectToAction("Index", "Login");
        }

        ViewBag.KullaniciAdi = kullaniciAdi;
        ViewBag.Rol = HttpContext.Session.GetString("Rol");
        return View();
    }

    public IActionResult CikisYap()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Login");
    }
}