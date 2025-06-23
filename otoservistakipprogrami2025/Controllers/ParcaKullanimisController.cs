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
    
    public class ParcaKullanimisController : Controller
    {
        private readonly OtoServisDbContext _context;

        public ParcaKullanimisController(OtoServisDbContext context)
        {
            _context = context;
        }

        // GET: ParcaKullanimis
        public async Task<IActionResult> Index()
        {
            var otoServisDbContext = _context.ParcaKullanimlari.Include(p => p.Parca).Include(p => p.ServisKaydi);
            return View(await otoServisDbContext.ToListAsync());
        }

        // GET: ParcaKullanimis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcaKullanimi = await _context.ParcaKullanimlari
                .Include(p => p.Parca)
                .Include(p => p.ServisKaydi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parcaKullanimi == null)
            {
                return NotFound();
            }

            return View(parcaKullanimi);
        }

        // GET: ParcaKullanimis/Create
        public IActionResult Create()
        {
            ViewData["ParcaId"] = new SelectList(_context.Parcalar, "Id", "ParcaAdi");
            ViewData["ServisKaydiId"] = new SelectList(_context.ServisKayitlari, "Id", "GelisSebebi");
            return View();
        }

        // POST: ParcaKullanimis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParcaId,ServisKaydiId,Miktar,BirimFiyat")] ParcaKullanimi parcaKullanimi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parcaKullanimi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParcaId"] = new SelectList(_context.Parcalar, "Id", "ParcaAdi", parcaKullanimi.ParcaId);
            ViewData["ServisKaydiId"] = new SelectList(_context.ServisKayitlari, "Id", "GelisSebebi", parcaKullanimi.ServisKaydiId);
            return View(parcaKullanimi);
        }

        // GET: ParcaKullanimis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcaKullanimi = await _context.ParcaKullanimlari.FindAsync(id);
            if (parcaKullanimi == null)
            {
                return NotFound();
            }
            ViewData["ParcaId"] = new SelectList(_context.Parcalar, "Id", "ParcaAdi", parcaKullanimi.ParcaId);
            ViewData["ServisKaydiId"] = new SelectList(_context.ServisKayitlari, "Id", "GelisSebebi", parcaKullanimi.ServisKaydiId);
            return View(parcaKullanimi);
        }

        // POST: ParcaKullanimis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParcaId,ServisKaydiId,Miktar,BirimFiyat")] ParcaKullanimi parcaKullanimi)
        {
            if (id != parcaKullanimi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parcaKullanimi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParcaKullanimiExists(parcaKullanimi.Id))
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
            ViewData["ParcaId"] = new SelectList(_context.Parcalar, "Id", "ParcaAdi", parcaKullanimi.ParcaId);
            ViewData["ServisKaydiId"] = new SelectList(_context.ServisKayitlari, "Id", "GelisSebebi", parcaKullanimi.ServisKaydiId);
            return View(parcaKullanimi);
        }

        // GET: ParcaKullanimis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcaKullanimi = await _context.ParcaKullanimlari
                .Include(p => p.Parca)
                .Include(p => p.ServisKaydi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parcaKullanimi == null)
            {
                return NotFound();
            }

            return View(parcaKullanimi);
        }

        // POST: ParcaKullanimis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parcaKullanimi = await _context.ParcaKullanimlari.FindAsync(id);
            if (parcaKullanimi != null)
            {
                _context.ParcaKullanimlari.Remove(parcaKullanimi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParcaKullanimiExists(int id)
        {
            return _context.ParcaKullanimlari.Any(e => e.Id == id);
        }
    }
}
