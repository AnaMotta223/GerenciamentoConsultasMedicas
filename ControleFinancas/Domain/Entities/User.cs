
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.ValueObjects;

namespace AppointmentsManager.Domain.Entities
{
    public abstract class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; set; }
        public string Password { get; private set; }
        public string Phone { get; private set; }
        public string Address {  get; private set; }
        public DateTime BirthDate { get; private set; }
        public Gender Gender { get; private set; }
        public CPF CPF { get; private set; }
        protected User()
        {
            
        }
        protected User(string name, string lastName, Email email, string password, string phone, string address, DateTime birthDate, Gender gender, CPF cpf)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            Password = password;
            Phone = phone;
            Address = address;
            BirthDate = birthDate;
            Gender = gender;
            CPF = cpf;
        }
    }
}
