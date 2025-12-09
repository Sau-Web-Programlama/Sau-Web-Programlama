using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Models
{
    public class AIDietViewModel
    {
        [Required(ErrorMessage = "Yaş gereklidir.")]
        [Range(15, 100, ErrorMessage = "Yaş 15-100 arasında olmalıdır.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Boy (cm) gereklidir.")]
        public int HeightCm { get; set; }

        [Required(ErrorMessage = "Kilo (kg) gereklidir.")]
        public decimal WeightKg { get; set; }

        [Required(ErrorMessage = "Hedef gereklidir.")]
        public string Goal { get; set; } // Kilo verme, Kas geliştirme, Koruma

        public string? ActivityLevel { get; set; } // Düşük, Orta, Yüksek (şimdilik kullanılmıyor)

        // YZ'den gelen sonuç bu değişkende tutulacak
        public string? DietPlanResult { get; set; }
    }
}