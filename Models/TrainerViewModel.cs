// Models/TrainerViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Models
{
    public class TrainerViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string Specialty { get; set; }
        public string Bio { get; set; }
    }
}