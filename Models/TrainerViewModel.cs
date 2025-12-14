<<<<<<< Updated upstream
﻿// Models/TrainerViewModel.cs
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
=======
﻿using System.ComponentModel.DataAnnotations;

namespace SporSalonu2.Models
{
    public class TrainerViewModel
    {
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur.")]
        [Display(Name = "Uzmanlık Alanı")]
        public string Specialty { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telefon Numarası")]
        public string Phone { get; set; }

        [Display(Name = "Biyografi/Tanıtım")]
>>>>>>> Stashed changes
        public string Bio { get; set; }
    }
}