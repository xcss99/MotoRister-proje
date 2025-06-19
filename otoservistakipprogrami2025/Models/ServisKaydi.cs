using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace otoservistakipprogrami2025.Models
{
    public class ServisKaydi
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Geliş tarihi zorunludur.")]
        [Display(Name = "Geliş Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime GelisTarihi { get; set; }

        [Display(Name = "Teslim Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime? TeslimTarihi { get; set; }

        [Required(ErrorMessage = "Geliş sebebi zorunludur.")]
        [Display(Name = "Geliş Sebebi")]
        public string? GelisSebebi { get; set; }

        [Display(Name = "Yapılan İşlemler")]
        public string? YapilanIslemler { get; set; }

        [Display(Name = "Toplam Tutar")]
        [DataType(DataType.Currency)]
        public decimal ToplamTutar { get; set; }

        [Display(Name = "Servis Durumu")]
        public string? ServisDurumu { get; set; } // Beklemede, İşlemde, Tamamlandı, Teslim Edildi

        // İlişkiler
        [Display(Name = "Araç")]
        public int AracId { get; set; }

        [ValidateNever]
        public virtual Arac Arac { get; set; }

        [ValidateNever]
        public virtual ICollection<ParcaKullanimi>? KullanilanParcalar { get; set; }
    }
}
