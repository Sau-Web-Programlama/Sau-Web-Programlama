using System.ComponentModel.DataAnnotations;

namespace SporSalonu2.Models
{
    
    public class Service
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } 

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(1, 300)]
        public int DurationMinutes { get; set; } 
        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }
}
}