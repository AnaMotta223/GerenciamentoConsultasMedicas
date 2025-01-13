using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Presentation.Models
{
    public class UpdateStatusAppointmentModel
    {
        public AppointmentStatus Status {  get; set; }
        public string Notes { get; set; }

        public UpdateStatusAppointmentDTO ToDto()
        {
            return new UpdateStatusAppointmentDTO
            {
                Status = this.Status,
                Notes = this.Notes
            };
        }
    }
}
