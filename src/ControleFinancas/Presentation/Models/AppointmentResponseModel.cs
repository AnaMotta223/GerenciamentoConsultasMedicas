using AppointmentsManager.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace AppointmentsManager.Presentation.Models
{
    public class AppointmentResponseModel
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string DoctorLastName { get; set; }
        public string PatientName { get; set; }
        public string PatientLastName { get; set; }

        [SwaggerSchema(Format = "date-time", Description = "Data e hora da consulta no formato dd/MM/yyyy HH:mm")]
        public string DateTimeAppointment { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public string Notes { get; set; }
    }
}
