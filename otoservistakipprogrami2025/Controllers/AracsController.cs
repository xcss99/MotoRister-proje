using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using otoservistakipprogrami2025.DAL;
using otoservistakipprogrami2025.Filters;
using otoservistakipprogrami2025.Models;

namespace otoservistakipprogrami2025.Controllers
{
    

    public class AracsController : Controller
    {
        private readonly OtoServisDbContext _context;

        public AracsController(OtoServisDbContext context)
        {
            _context = context;
        }

        // GET: Aracs
        public async Task<IActionResult> Index()
        {
            var otoServisDbContext = _context.Araclar.Include(a => a.Musteri);
            return View(await otoServisDbContext.ToListAsync());
        }

        // GET: Aracs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arac = await _context.Araclar
                .Include(a => a.Musteri)
                .FirstOrDefaultAsync(m => m.AracId == id);
            if (arac == null)
            {
                return NotFound();
            }

            return View(arac);
        }

        // GET: Aracs/Create
        public IActionResult Create(int? MusteriId)
        {
           
            {
                ViewData["MusteriId"] = new SelectList(_context.Musteriler, "MusteriId", "Ad", MusteriId);

                // Eğer müşteri seçilmişse, o müşteriyi default olarak seç
                if (MusteriId.HasValue)
                {
                    ViewBag.SelectedMusteriId = MusteriId.Value;
                    var musteri = _context.Musteriler.Find(MusteriId.Value);
                    ViewBag.MusteriAdi = musteri?.Ad + " " + musteri?.Soyad;
                }

                return View();
            }
        }

        // POST: Aracs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AracId,AracPlaka,AracMarka,AracModel,AracModelYili,AracSasiNo,AracMotorNo,AracRenk,MusteriId")] Arac arac)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Hata: {modelState.Key} => {error.ErrorMessage}");
                    }
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(arac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MusteriId"] = new SelectList(_context.Musteriler, "MusteriId", "Ad", arac.MusteriId);
            return View(arac);
        }

        // GET: Aracs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arac = await _context.Araclar.FindAsync(id);
            if (arac == null)
            {
                return NotFound();
            }
            ViewData["MusteriId"] = new SelectList(_context.Musteriler, "MusteriId", "Ad", arac.MusteriId);
            return View(arac);
        }

        // POST: Aracs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AracId,AracPlaka,AracMarka,AracModel,AracModelYili,AracSasiNo,AracMotorNo,AracRenk,MusteriId")] Arac arac)
        {
            if (id != arac.AracId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(arac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AracExists(arac.AracId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MusteriId"] = new SelectList(_context.Musteriler, "MusteriId", "Ad", arac.MusteriId);
            return View(arac);
        }

        // GET: Aracs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arac = await _context.Araclar
                .Include(a => a.Musteri)
                .FirstOrDefaultAsync(m => m.AracId == id);
            if (arac == null)
            {
                return NotFound();
            }

            return View(arac);
        }

        // POST: Aracs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var arac = await _context.Araclar.FindAsync(id);
            if (arac != null)
            {
                _context.Araclar.Remove(arac);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AracExists(int id)
        {
            return _context.Araclar.Any(e => e.AracId == id);
        }
    }
}
