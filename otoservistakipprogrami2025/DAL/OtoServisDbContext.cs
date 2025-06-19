
using otoservistakipprogrami2025.Models;
using Microsoft.EntityFrameworkCore;

namespace otoservistakipprogrami2025.DAL
{
    public class OtoServisDbContext : DbContext
    {
        public OtoServisDbContext(DbContextOptions<OtoServisDbContext> options) 
            : base(options)

        {
        }

        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Arac> Araclar { get; set; }
        public DbSet<ServisKaydi> ServisKayitlari { get; set; }
        public DbSet<Parca> Parcalar { get; set; }
        public DbSet<ParcaKullanimi> ParcaKullanimlari { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Musteri>().ToTable();
            // Musteri-Arac ilişkisi (1-n)

            modelBuilder.Entity<Arac>()
                .HasOne(a => a.Musteri)
                .WithMany(m => m.Araclar)
                .HasForeignKey(a => a.MusteriId);

            // Arac-ServisKaydi ilişkisi (1-n)
            modelBuilder.Entity<ServisKaydi>()
                .HasOne(s => s.Arac)
                .WithMany(a => a.ServisKayitlari)
                .HasForeignKey(s => s.AracId);

            // ServisKaydi-ParcaKullanimi-Parca ilişkisi (n-n)
            modelBuilder.Entity<ParcaKullanimi>()
                .HasOne(pk => pk.Parca)
                .WithMany(p => p.Kullanimlar)
                .HasForeignKey(pk => pk.ParcaId);
            //parça kullanımı (n-1)
            modelBuilder.Entity<ParcaKullanimi>()
                .HasOne(pk => pk.ServisKaydi)
                .WithMany(s => s.KullanilanParcalar)
                .HasForeignKey(pk => pk.ServisKaydiId);

            //base.OnModelCreating(modelBuilder);
        }
    }
}
