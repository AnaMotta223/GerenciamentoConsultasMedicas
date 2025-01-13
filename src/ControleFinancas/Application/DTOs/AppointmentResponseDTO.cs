using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Application.DTOs
{
    public class AppointmentResponseDTO
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId  { get; set; }
        public string DateTimeAppointment { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public string Notes { get; set; }
    }
}
