using System.ComponentModel.DataAnnotations;
using System;

namespace SporSalonu2.Models
{
   
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public string MemberId { get; set; } // Üye ID'si (ASP.NET Identity kullanıldığı varsayılıyor)

        [Required]
        public int TrainerId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; } // Randevu tarihi

        [Required]
        [MaxLength(5)]
        public string AppointmentTime { get; set; } // Randevu saati (string tutulabilir: "10:00")

        [MaxLength(1000)]
        public string Notes { get; set; }

        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Pending; // Randevu onay durumu [cite: 21]

        // İlişkiler
        public Trainer Trainer { get; set; }
        public Service Service { get; set; }
        // User (Member) ilişkisi, ASP.NET Identity kurulduktan sonra eklenecek.
    }

    public enum BookingStatus
    {
        Pending, // Beklemede (Onay mekanizması) [cite: 21]
        Approved, // Onaylandı
        Rejected, // Reddedildi
        Cancelled // İptal edildi (Üye tarafından)
    }
}