using Microsoft.AspNetCore.Mvc;

namespace otoservistakipprogrami2025.Models
{
    
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
