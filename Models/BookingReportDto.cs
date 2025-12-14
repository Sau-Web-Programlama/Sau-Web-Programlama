using System;

namespace SporSalonu2.Models
{
    // API üzerinden dışarıya aktarılacak randevu raporlama verisi
    public class BookingReportDto
    {
        public int Id { get; set; }
        public string MemberEmail { get; set; }
        public string TrainerName { get; set; }
        public string ServiceName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string Status { get; set; } // Onaylandı, Beklemede, vb.
    }
}