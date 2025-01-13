using AppointmentsManager.Application.DTOs;

namespace AppointmentsManager.Presentation.Models
{
    public class CreateAppointmentModel
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime DateTimeAppointment { get; set; }

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
