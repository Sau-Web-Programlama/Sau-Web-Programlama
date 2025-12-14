using System.ComponentModel.DataAnnotations;

namespace SporSalonu2.Models
{
    public class ServiceViewModel
    {
        // Service modelindeki alanları yansıtır
        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Süre (dk) zorunludur.")]
        [Range(1, 300, ErrorMessage = "Süre 1 ile 300 dakika arasında olmalıdır.")]
        [Display(Name = "Süre (Dakika)")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(0.01, 10000.00, ErrorMessage = "Fiyat geçerli bir değer olmalıdır.")]
        [Display(Name = "Fiyat (TL)")]
        public decimal Price { get; set; }
    }
}