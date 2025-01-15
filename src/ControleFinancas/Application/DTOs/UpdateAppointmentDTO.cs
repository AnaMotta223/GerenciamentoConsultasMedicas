using AppointmentsManager.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace AppointmentsManager.Application.DTOs
{
    public class UpdateAppointmentDTO
    {
        [SwaggerSchema(Format = "date-time", Description = "Data e hora da consulta no formato dd/MM/yyyy HH:mm")]
        public string DateTimeAppointment { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public string Notes { get; set; }
    }
}
