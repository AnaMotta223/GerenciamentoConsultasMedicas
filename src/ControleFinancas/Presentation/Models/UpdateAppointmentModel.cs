using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace AppointmentsManager.Presentation.Models
{
    public class UpdateAppointmentModel
    {
        [SwaggerSchema(Format = "date-time", Description = "Data e hora da consulta no formato dd/MM/yyyy HH:mm")]
        public string DateTimeAppointment { get; set; }
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
