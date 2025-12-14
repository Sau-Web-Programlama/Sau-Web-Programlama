using System;
using System.ComponentModel.DataAnnotations;

namespace SporSalonu2.Models
{

    public class Availability
    {
        public int Id { get; set; }

        [Required]
        public int TrainerId { get; set; }


        public Trainer Trainer { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; } 

        [Required]
        public TimeSpan StartTime { get; set; } 

        [Required]
        public TimeSpan EndTime { get; set; } 

        public bool IsBookable { get; set; } = true; 
    }


    public enum DayOfWeek
    {
        Pazartesi, Sali, Carsamba, Persembe, Cuma, Cumartesi, Pazar
    }
}