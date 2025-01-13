using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Presentation.Models
{
    public class CreateDoctorModel
    {
        public string Name {  get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string CPF {  get; set; }
        public Speciality Speciality { get; set; }
        public string RMC { get; set; }

        public CreateDoctorDTO ToDto()
        {
            return new CreateDoctorDTO
            {
                Name = this.Name,
                LastName = this.LastName,
                Email = this.Email,
                Password = this.Password,
                Phone = this.Phone,
                Address = this.Address,
                BirthDate = this.BirthDate,
                Gender = this.Gender,
                CPF = this.CPF,
                Speciality = this.Speciality,
                RMC = this.RMC
            };
        }
    }
}
