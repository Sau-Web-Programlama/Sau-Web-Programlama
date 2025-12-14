// Models/RegisterViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad gereklidir.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gereklidir.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon gereklidir.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifre Tekrar")]
        public string ConfirmPassword { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Kullanım şartlarını kabul etmelisiniz.")]
        public bool AcceptTerms { get; set; }
    }
}