using Swashbuckle.AspNetCore.Annotations;

namespace AppointmentsManager.Application.DTOs
{
    public class CreateAppointmentDTO
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        [SwaggerSchema(Format = "date-time", Description = "Data e hora da consulta no formato dd/MM/yyyy HH:mm")]
        public string DateTimeAppointment { get; set; }
    }
}
