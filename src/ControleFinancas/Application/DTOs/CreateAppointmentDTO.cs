using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Application.DTOs
{
    public class CreateAppointmentDTO
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime DateTimeAppointment { get; set; }
    }
}
