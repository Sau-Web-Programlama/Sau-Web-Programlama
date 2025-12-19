using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SporSalonu2.Models
{
   
    public class Trainer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(300)]
        public string Specialty { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; } 

        [MaxLength(1000)]
        public string Bio { get; set; }

        
        
        public string WorkingDays { get; set; }
    }

}
