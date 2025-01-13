using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.ValueObjects;

namespace AppointmentsManager.Domain.Entities
{
    public class Doctor : User
    {
        public Speciality Speciality { get; set; }
        public RMC RMC { get; set; }
        public List<DateTimeWork> DateTimeWorkList { get; set; }
        public List<Appointment> Appointments { get; set; } = new();

        public Doctor() { }
        public Doctor(string name, string lastName, string password, string address, DateTime birthDate, Gender gender, Email email, string phone, CPF cpf, RMC rmc, Speciality speciality, List<DateTimeWork> dateTimeWorkList)
         : base(name, lastName, email, password, phone, address, birthDate, gender, cpf)
        {
            Speciality = speciality;
            DateTimeWorkList = dateTimeWorkList;
            RMC = rmc ?? throw new ArgumentNullException(nameof(rmc));
        }
        public Doctor(string name, string lastName, string password, string address, DateTime birthDate, Gender gender, Email email, string phone, CPF cpf, RMC rmc, Speciality speciality, List<DateTimeWork> dateTimeWorkList, List<Appointment> appointments)
        : base(name, lastName, email, password, phone, address, birthDate, gender, cpf)
        {
            Speciality = speciality;
            DateTimeWorkList = dateTimeWorkList;
            RMC = rmc ?? throw new ArgumentNullException(nameof(rmc));
        }
        public bool IsWithinWorkHours(DateTime appointmentDateTime)
        {
            var dayOfWeek = appointmentDateTime.DayOfWeek;

            // Encontra os horários de trabalho para o dia da semana
            //verificar comparacao
            var workHours = DateTimeWorkList.FirstOrDefault(w => w.DayOfWeek == (int)dayOfWeek);
            if (workHours == null)
            {
                //horarios = null -> nao trabalha
                Console.WriteLine("o médico não trabalha nesse dia");
                return false;
                
            }
            return appointmentDateTime.TimeOfDay >= workHours.StartTime &&
                   appointmentDateTime.TimeOfDay <= workHours.EndTime;
        }
        public bool HasConflict(DateTime appointmentDateTime)
        {
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
