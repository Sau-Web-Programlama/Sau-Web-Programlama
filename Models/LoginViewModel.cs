// Models/LoginViewModel.cs
using System.ComponentModel.DataAnnotations;

<<<<<<< Updated upstream
namespace FitnessCenter.Models
=======
namespace SporSalonu2.Models
>>>>>>> Stashed changes
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}