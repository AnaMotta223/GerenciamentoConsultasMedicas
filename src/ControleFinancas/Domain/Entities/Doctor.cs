using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.ValueObjects;

namespace AppointmentsManager.Domain.Entities
{
    public class Doctor : User
    {
        public Speciality Speciality { get; set; }
        public RMC RMC { get; set; }
        public List<DoctorDateTimeWorkResponseDTO> DateTimeWorkList { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
        public Doctor() { }
        public Doctor(string name, string lastName, string password, string address, DateTime birthDate, Gender gender, Email email, string phone, CPF cpf, RMC rmc, Speciality speciality, List<DoctorDateTimeWorkResponseDTO> dateTimeWorkList)
         : base(name, lastName, email, password, phone, address, birthDate, gender, cpf)
        {
            RMC = rmc;
            Speciality = speciality;
            DateTimeWorkList = dateTimeWorkList;
        }
        public Doctor(string name, string lastName, string password, string address, DateTime birthDate, Gender gender, Email email, string phone, CPF cpf, RMC rmc, Speciality speciality, List<DoctorDateTimeWorkResponseDTO> dateTimeWorkList, List<Appointment> appointments)
        : base(name, lastName, email, password, phone, address, birthDate, gender, cpf)
        {
            Speciality = speciality;
           DateTimeWorkList = dateTimeWorkList;
        }
        public bool IsWithinWorkHours(DateTime appointmentDateTime, List<DoctorDateTimeWorkResponseDTO> dateTimeWorkList)
        {
            DateTimeWorkList = dateTimeWorkList;
            var dayOfWeek = appointmentDateTime.DayOfWeek;

            var workHours = DateTimeWorkList.FirstOrDefault(w => w.DayOfWeek == (int)dayOfWeek);
            if (workHours == null)
            {
                return false;
                
            }
            return appointmentDateTime.TimeOfDay >= workHours.StartTime.ToTimeSpan() &&
                   appointmentDateTime.TimeOfDay <= workHours.EndTime.ToTimeSpan();
        }
        public bool HasConflict(DateTime appointmentDateTime, List<Appointment> appointments)
        {
            Appointments = appointments;
            return Appointments.Any(a => a.DateTimeAppointment == appointmentDateTime && a.AppointmentStatus != AppointmentStatus.CANCELED);
        }
        public override bool Equals(object? obj)
        {
            return obj is Doctor doctor &&
                   Id == doctor.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
