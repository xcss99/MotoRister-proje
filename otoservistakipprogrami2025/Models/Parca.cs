using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace otoservistakipprogrami2025.Models
{
    public class Parca
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Parça kodu zorunludur.")]
        [Display(Name = "Parça Kodu")]
        public string?  ParcaKodu { get; set; }

        [Required(ErrorMessage = "Parça adı zorunludur.")]
        [Display(Name = "Parça Adı")]
        public string? ParcaAdi { get; set; }

        [Required(ErrorMessage = "Birim fiyat zorunludur.")]
        [Display(Name = "Birim Fiyat")]
        [DataType(DataType.Currency)]
        public decimal BirimFiyat { get; set; }

        [Required(ErrorMessage = "Stok miktarı zorunludur.")]
        [Display(Name = "Stok Miktarı")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0'dan küçük olamaz.")]
        public int StokMiktari { get; set; }

        // İlişkiler
        [ValidateNever]
        public virtual ICollection<ParcaKullanimi>? Kullanimlar { get; set; }
    }
}