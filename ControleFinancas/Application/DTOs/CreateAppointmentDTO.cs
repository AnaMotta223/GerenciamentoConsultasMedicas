using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Application.DTOs
{
    public class CreateAppointmentDTO
    {
        public int Id { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public DateTime DateTimeAppointment { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
    }
}
