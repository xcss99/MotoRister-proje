using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using otoservistakipprogrami2025.Models;
using otoservistakipprogrami2025.DAL;
using otoservistakipprogrami2025.Filters;

namespace otoservistakipprogrami2025.Controllers
{
    [AdminAuthorize]

    public class MusteriController : Controller
    {
        private readonly OtoServisDbContext _context;

        public MusteriController(OtoServisDbContext context)
        {
            _context = context;
        }

        // GET: Musteri
        public async Task<IActionResult> Index()
        {
            var musteriler = await _context.Musteriler
                .Include(m => m.Araclar)
                .ToListAsync();

            return View(musteriler);
        }


        // GET: Musteri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musteri = await _context.Musteriler
                .FirstOrDefaultAsync(m => m.MusteriId == id);

            if (musteri == null)
            {
                return NotFound();
            }

            return View(musteri);
        }

        // GET: Musteri/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Musteri/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ad,Soyad,Telefon,Email,Adres,KayitTarihi")] Musteri musteri)
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
                musteri.KayitTarihi = DateTime.Now;
                _context.Musteriler.Add(musteri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(musteri);
        }

        // GET: Musteri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musteri = await _context.Musteriler.FindAsync(id);
            if (musteri == null)
            {
                return NotFound();
            }
            return View(musteri);
        }

        // POST: Musteri/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MusteriId,Ad,Soyad,Telefon,Email,Adres")] Musteri musteri)
        {
            if (id != musteri.MusteriId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Musteriler.Update(musteri);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusteriExists(musteri.MusteriId))
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
            return View(musteri);
        }

        // GET: Musteri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musteri = await _context.Musteriler
                .FirstOrDefaultAsync(m => m.MusteriId == id);
            if (musteri == null)
            {
                return NotFound();
            }

            return View(musteri);
        }

        // POST: Musteri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musteri = await _context.Musteriler.FindAsync(id);

            if (musteri == null)
            {
                return NotFound(); // Veya RedirectToAction(nameof(Index));
            }

            _context.Musteriler.Remove(musteri);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusteriExists(int id)
        {
            return _context.Musteriler.Any(e => e.MusteriId == id);
        }
    }
}