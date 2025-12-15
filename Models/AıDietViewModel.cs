using System.ComponentModel.DataAnnotations;

namespace SporSalonu2.Models
{
    public class AIDietViewModel
    {
        [Required(ErrorMessage = "Yaş gereklidir.")]
        [Range(10, 100, ErrorMessage = "Yaş 10-100 arasında olmalıdır.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Boy gereklidir.")]
        public int HeightCm { get; set; }

        [Required(ErrorMessage = "Kilo gereklidir.")]
        public decimal WeightKg { get; set; }

        [Required(ErrorMessage = "Hedef seçilmelidir.")]
        public string Goal { get; set; }

        public string? DietPlanResult { get; set; }
    }
}
