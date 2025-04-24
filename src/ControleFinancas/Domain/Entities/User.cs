
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.ValueObjects;

namespace AppointmentsManager.Domain.Entities
{
    public abstract class User
    {
        public int Id { get; set; }
        public UserStatus Status { get; set; }
        public string Name { get;  set; }
        public string LastName { get;  set; }
        public Email Email { get; set; }
        public string Password { get;  set; }
        public string Phone { get;  set; }
        public string Address {  get;  set; }
        public DateTime BirthDate { get;  set; }
        public Gender Gender { get;  set; }
        public CPF CPF { get;  set; }

        protected User()
        {
            
        }
        protected User(UserStatus status, string name, string lastName, Email email, string password, string phone, string address, DateTime birthDate, Gender gender, CPF cpf)
        {
            Status = status;
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
