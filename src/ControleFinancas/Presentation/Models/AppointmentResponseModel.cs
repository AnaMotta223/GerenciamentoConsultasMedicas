using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Presentation.Models
{
    public class AppointmentResponseModel
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string DoctorLastName { get; set; }
        public string PatientName { get; set; }
        public string PatientLastName { get; set; }
        public string DateTimeAppointment { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public string Notes { get; set; }
    }
}
