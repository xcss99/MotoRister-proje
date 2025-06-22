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
    public class ServisKaydisController : Controller
    {
        private readonly OtoServisDbContext _context;

        public ServisKaydisController(OtoServisDbContext context)
        {
            _context = context;
        }

        // GET: ServisKaydis
        public async Task<IActionResult> Index()
        {
            var otoServisDbContext = _context.ServisKayitlari.Include(s => s.Arac);
            return View(await otoServisDbContext.ToListAsync());
        }

        // GET: ServisKaydis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servisKaydi = await _context.ServisKayitlari
                .Include(s => s.Arac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servisKaydi == null)
            {
                return NotFound();
            }

            return View(servisKaydi);
        }

        // GET: ServisKaydis/Create
        public IActionResult Create()
        {
            ViewData["AracId"] = new SelectList(_context.Araclar, "AracId", "AracMarka");
            return View();
        }

        // POST: ServisKaydis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GelisTarihi,TeslimTarihi,GelisSebebi,YapilanIslemler,ToplamTutar,ServisDurumu,AracId")] ServisKaydi servisKaydi)
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
                _context.Add(servisKaydi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AracId"] = new SelectList(_context.Araclar, "AracId", "AracMarka", servisKaydi.AracId);
            return View(servisKaydi);
        }

        // GET: ServisKaydis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servisKaydi = await _context.ServisKayitlari.FindAsync(id);
            if (servisKaydi == null)
            {
                return NotFound();
            }
            ViewData["AracId"] = new SelectList(_context.Araclar, "AracId", "AracMarka", servisKaydi.AracId);
            return View(servisKaydi);
        }

        // POST: ServisKaydis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GelisTarihi,TeslimTarihi,GelisSebebi,YapilanIslemler,ToplamTutar,ServisDurumu,AracId")] ServisKaydi servisKaydi)
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
            if (id != servisKaydi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servisKaydi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServisKaydiExists(servisKaydi.Id))
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
            ViewData["AracId"] = new SelectList(_context.Araclar, "AracId", "AracMarka", servisKaydi.AracId);
            return View(servisKaydi);
        }

        // GET: ServisKaydis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servisKaydi = await _context.ServisKayitlari
                .Include(s => s.Arac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servisKaydi == null)
            {
                return NotFound();
            }

            return View(servisKaydi);
        }

        // POST: ServisKaydis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servisKaydi = await _context.ServisKayitlari.FindAsync(id);
            if (servisKaydi != null)
            {
                _context.ServisKayitlari.Remove(servisKaydi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServisKaydiExists(int id)
        {
            return _context.ServisKayitlari.Any(e => e.Id == id);
        }
    }
}
