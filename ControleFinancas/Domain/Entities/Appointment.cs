using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public DateTime DateTime { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public Appointment()
        {
            
        }
        public Appointment(Doctor doctor, Patient patient, DateTime dateTime, AppointmentStatus appointmentStatus)
        {
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            DateTime = dateTime;
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
