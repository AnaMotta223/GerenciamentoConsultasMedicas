using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.ValueObjects;

namespace AppointmentsManager.Domain.Entities
{
    public class Patient : User
    {
        public Patient() { }

        public Patient(string name, string lastName, string password, string address, DateTime birthDate, Gender gender, Email email, string phone, CPF cpf)
            : base(name, lastName, email, password, phone, address, birthDate, gender, cpf)
        {

        }
        public override bool Equals(object? obj)
        {
            return obj is Patient patient &&
                   Id == patient.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
