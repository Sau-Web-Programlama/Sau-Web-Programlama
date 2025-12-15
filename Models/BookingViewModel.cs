// Models/BookingViewModel.cs
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SporSalonu2.Models
{
    public class BookingViewModel
    {
        [Required(ErrorMessage = "Hizmet tipi seçimi zorunludur.")]
        // public string ServiceType { get; set; } // ÖNCEKİ HALİ
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir hizmet seçimi zorunludur.")]
        public int ServiceType { get; set; }

        [Required(ErrorMessage = "Antrenör seçimi zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir antrenör seçiniz.")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Tarih seçimi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Saat seçimi zorunludur.")]
        public string AppointmentTime { get; set; }

        public string Notes { get; set; }

        public List<SelectListItemDto> Trainers { get; set; }
        public List<SelectListItemDto> Services { get; set; }
    }
}