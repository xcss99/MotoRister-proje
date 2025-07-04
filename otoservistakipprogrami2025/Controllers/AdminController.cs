﻿using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        
        var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
       // if (string.IsNullOrEmpty(kullaniciAdi))
        //{
            // Login sayfasına yönlendir, Admin'e değil!
         //   return RedirectToAction("Index", "Login");
       // }

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