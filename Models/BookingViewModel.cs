// Models/BookingViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Models
{
    public class BookingViewModel
    {
        [Required(ErrorMessage = "Hizmet tipi seçimi zorunludur.")]
        public string ServiceType { get; set; }

        [Required(ErrorMessage = "Antrenör seçimi zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir antrenör seçiniz.")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Tarih seçimi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Saat seçimi zorunludur.")]
        public string AppointmentTime { get; set; }

        public string Notes { get; set; }
    }
}