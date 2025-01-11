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
