using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace otoservistakipprogrami2025.Models
{
    public class Arac
    {
        public int AracId { get; set; }

        [Required(ErrorMessage = "Plaka alanı zorunludur.")]
        [Display(Name = "Plaka")]
        public string?  AracPlaka { get; set; }

        [Required(ErrorMessage = "Marka alanı zorunludur.")]
        [Display(Name = "Marka")]
        public string? AracMarka { get; set; }

        [Required(ErrorMessage = "Model alanı zorunludur.")]
        [Display(Name = "Model")]
        public string? AracModel { get; set; }

        [Required(ErrorMessage = "Model Yılı alanı zorunludur.")]
        [Display(Name = "Model Yılı")]
        [Range(1950, 2100, ErrorMessage = "Geçerli bir model yılı giriniz.")]
        public int AracModelYili { get; set; }

        [Display(Name = "Şasi No")]
        public string? AracSasiNo { get; set; }

        [Display(Name = "Motor No")]
        public string? AracMotorNo { get; set; }

        [Display(Name = "Renk")]
        public string? AracRenk { get; set; }

        // İlişkiler
        
        [Display(Name = "Müşteri")]
        public int MusteriId { get; set; }

        [ValidateNever]
        public virtual Musteri? Musteri { get; set; }

        [ValidateNever]
        public virtual ICollection<ServisKaydi>? ServisKayitlari { get; set; }

        // Plaka, marka ve model birleşimi için yardımcı özellik
        [Display(Name = "Araç")]
        public string PlakaMarkaModel => $"{AracPlaka} - {AracMarka} {AracModel}";

    }
}