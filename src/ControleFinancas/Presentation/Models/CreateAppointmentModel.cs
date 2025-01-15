using AppointmentsManager.Application.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace AppointmentsManager.Presentation.Models
{
    public class CreateAppointmentModel
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        [SwaggerSchema(Format = "date-time", Description = "Data e hora da consulta no formato dd/MM/yyyy HH:mm")]
        public string DateTimeAppointment { get; set; }
        public CreateAppointmentDTO ToDto()
        {
            return new CreateAppointmentDTO
            {
                PatientId = this.PatientId,
                DoctorId = this.DoctorId,
                DateTimeAppointment = this.DateTimeAppointment
            };
        }
    }
}
