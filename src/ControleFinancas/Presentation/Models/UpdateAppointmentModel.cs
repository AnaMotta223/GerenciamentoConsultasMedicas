using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Presentation.Models
{
    public class UpdateAppointmentModel
    {
        public DateTime DateTimeAppointment { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public string Notes { get; set; }

        public UpdateAppointmentDTO ToDto()
        {
            return new UpdateAppointmentDTO
            {
                DateTimeAppointment = this.DateTimeAppointment,
                AppointmentStatus = this.AppointmentStatus,
                Notes = this.Notes
            };
        }
    }
}
