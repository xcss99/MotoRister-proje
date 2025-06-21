using Microsoft.AspNetCore.Mvc;
using otoservistakipprogrami2025.Filters;

namespace otoservistakipprogrami2025.Models
{
    [AdminAuthorize]

    public class User 
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; } // Gerçekte hash'lenmiş olmalı
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public DateTime KayitTarihi { get; set; }
        public bool IsActive { get; set; }
    }
}
