using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // Bu kütüphaneyi ekledik

namespace SporSalonu2.Models
{
    public class BookingViewModel
    {
        [Required(ErrorMessage = "Hizmet tipi seçimi zorunludur.")]
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

        // --- DEĞİŞİKLİK BURADA ---
        // [ValidateNever] ekledik: "Form gönderilirken bu listelerin dolu olup olmadığını kontrol etme" demek.
        // Ayrıca tiplerin yanına '?' koyarak boş olabileceklerini belirttik.

        [ValidateNever]
        public List<SelectListItemDto>? Trainers { get; set; }

        [ValidateNever]
        public List<SelectListItemDto>? Services { get; set; }
    }
}