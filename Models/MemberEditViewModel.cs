using System.ComponentModel.DataAnnotations;

namespace SporSalonu2.Models
{
    public class MemberEditViewModel
    {
        // HATANIN SEBEBİ BURASIYDI (string yerine int yaptık)
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Telefon")]
        public string Phone { get; set; }
    }
}