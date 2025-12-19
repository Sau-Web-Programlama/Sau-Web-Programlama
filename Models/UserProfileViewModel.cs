using System.ComponentModel.DataAnnotations;

namespace SporSalonu2.Models
{
    public class UserProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } // Genelde email değiştirilmez, sadece okunur yapılır

        [Display(Name = "Telefon Numarası")]
        public string Phone { get; set; }

        [Display(Name = "Boy (cm)")]
        public double? Height { get; set; }

        [Display(Name = "Kilo (kg)")]
        public double? Weight { get; set; }
    }
}