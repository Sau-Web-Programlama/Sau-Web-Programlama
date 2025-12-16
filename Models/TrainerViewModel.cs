using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SporSalonu2.Models
{
    public class TrainerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        // --- DEĞİŞİKLİK BURADA ---
        // Eskiden burada [Required] vardı, onu SİLDİK.
        // Çünkü bu alan artık formdan gelmiyor, Controller'da biz oluşturuyoruz.
        public string? Specialty { get; set; }
        // -------------------------

        // Formdan gelen asıl veri bu:
        [Display(Name = "Uzmanlık Alanları")]
        public List<int> SelectedServiceIds { get; set; } = new List<int>();

        // Checkbox listesini doldurmak için kullanılan liste:
        public List<SelectListItemDto> AvailableServices { get; set; } = new List<SelectListItemDto>();

        [EmailAddress]
        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telefon Numarası")]
        public string Phone { get; set; }

        [Display(Name = "Biyografi")]
        public string Bio { get; set; }
    }
}