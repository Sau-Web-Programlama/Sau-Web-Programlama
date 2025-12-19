using System.ComponentModel.DataAnnotations;

namespace SporSalonu2.Models
{
    public class ServiceViewModel
    {
        // BU SATIR EKSİK OLABİLİR, MUTLAKA EKLE:
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Süre (dk)")]
        public int DurationMinutes { get; set; }

        [Required]
        [Display(Name = "Ücret (₺)")]
        public decimal Price { get; set; }
    }
}