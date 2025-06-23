using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using otoservistakipprogrami2025.DAL;
using otoservistakipprogrami2025.Models;

namespace otoservistakipprogrami2025.Controllers
{
    public class ParcasController : Controller
    {
        private readonly OtoServisDbContext _context;

        public ParcasController(OtoServisDbContext context)
        {
            _context = context;
        }

        // GET: Parcas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parcalar.ToListAsync());
        }

        // GET: Parcas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parca = await _context.Parcalar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parca == null)
            {
                return NotFound();
            }

            return View(parca);
        }

        // GET: Parcas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parcas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParcaKodu,ParcaAdi,BirimFiyat,StokMiktari")] Parca parca)
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
                _context.Add(parca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parca);
        }

        // GET: Parcas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parca = await _context.Parcalar.FindAsync(id);
            if (parca == null)
            {
                return NotFound();
            }
            return View(parca);
        }

        // POST: Parcas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParcaKodu,ParcaAdi,BirimFiyat,StokMiktari")] Parca parca)
        {
            if (id != parca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParcaExists(parca.Id))
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
            return View(parca);
        }

        // GET: Parcas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parca = await _context.Parcalar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parca == null)
            {
                return NotFound();
            }

            return View(parca);
        }

        // POST: Parcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parca = await _context.Parcalar.FindAsync(id);
            if (parca != null)
            {
                _context.Parcalar.Remove(parca);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParcaExists(int id)
        {
            return _context.Parcalar.Any(e => e.Id == id);
        }
    }
}
