using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Application.DTOs
{
    public class UpdateAppointmentDTO
    {
        public DateTime DateTimeAppointment { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public string Notes { get; set; }
    }
}
