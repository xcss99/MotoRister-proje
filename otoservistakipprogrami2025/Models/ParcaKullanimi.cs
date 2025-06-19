using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace otoservistakipprogrami2025.Models
{
    public class ParcaKullanimi
    {
        public int Id { get; set; }

        [Display(Name = "Parça")]
        public int ParcaId { get; set; }

        [Display(Name = "Servis Kaydı")]
        public int ServisKaydiId { get; set; }

        [Required(ErrorMessage = "Miktar zorunludur.")]
        [Display(Name = "Miktar")]
        [Range(1, int.MaxValue, ErrorMessage = "Miktar en az 1 olmalıdır.")]
        public int Miktar { get; set; }

        [Display(Name = "Birim Fiyat")]
        [DataType(DataType.Currency)]
        public decimal BirimFiyat { get; set; }

        // İlişkiler
        [ValidateNever]
        public virtual Parca Parca { get; set; }

        [ValidateNever]
        public virtual ServisKaydi ServisKaydi { get; set; }

        // Toplam fiyat için hesaplanan özellik
        [Display(Name = "Toplam Fiyat")]
        public decimal ToplamFiyat
        {
            get { return BirimFiyat * Miktar; }
        }
    }
}
