using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.ValueObjects;

namespace AppointmentsManager.Application.DTOs
{
    public class CreatePatientDTO
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public Email Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public CPF CPF { get; set; }
    }
}
