using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Application.DTOs
{
    public class UpdateStatusAppointmentDTO
    {
        public AppointmentStatus Status {  get; set; }
        public string Notes { get; set; }
    }
}
