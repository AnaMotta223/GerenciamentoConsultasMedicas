using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime DateTimeAppointment { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public string Notes { get; set; }
        public Appointment()
        {
            
        }
        public Appointment(int doctorId, int patientId, DateTime dateTime, AppointmentStatus appointmentStatus, string notes)
        {
            DoctorId = doctorId; 
            PatientId = patientId;
            DateTimeAppointment = dateTime;
            AppointmentStatus = appointmentStatus;
            Notes = notes;
        }

        public Appointment(int doctorId, int patientId, DateTime dateTime, AppointmentStatus appointmentStatus)
        {
            DoctorId = doctorId;
            PatientId = patientId;
            DateTimeAppointment = dateTime;
            AppointmentStatus = appointmentStatus;
        }
       public override bool Equals(object? obj)
        {
            return obj is Appointment appointment &&
                   Id == appointment.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
