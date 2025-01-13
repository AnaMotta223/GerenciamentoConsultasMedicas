using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Presentation.Models
{
    public class UpdateDoctorModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RMC { get; set; }
        public Speciality Speciality { get; set; }

        public UpdateDoctorDTO ToDto()
        {
            return new UpdateDoctorDTO
            {
                Name = this.Name,
                LastName = this.LastName,
                Password = this.Password,
                Address = this.Address,
                BirthDate = this.BirthDate,
                Gender = this.Gender,
                Email = this.Email,
                Phone = this.Phone,
                RMC = this.RMC,
                Speciality = this.Speciality
            };
        }
}
