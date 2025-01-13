using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.ValueObjects;

namespace AppointmentsManager.Presentation.Models
{
    public class CreatePatientModel
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

        public CreatePatientDTO ToDto()
        {
            return new CreatePatientDTO
            {
                Name = this.Name,
                LastName = this.LastName,
                Email = this.Email,
                Password = this.Password,
                Phone = this.Phone,
                Address = this.Address,
                BirthDate = this.BirthDate,
                Gender = this.Gender,
                CPF = this.CPF
            };
        }
    }
}
