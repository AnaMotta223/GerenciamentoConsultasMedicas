using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AppointmentsManager.Presentation.Models
{
    public class UpdateDoctorModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        [SwaggerSchema(Format = "date", Description = "Data de nascimento no formato dd/MM/yyyy")]
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = "O formato da data deve ser dd/MM/yyyy.")]
        public string BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string CPF { get; set; }
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
                Address = this.Address,
                BirthDate = this.BirthDate,
                Gender = this.Gender,
                CPF = this.CPF,
                Email = this.Email,
                Phone = this.Phone,
                RMC = this.RMC,
                Speciality = this.Speciality
            };
        }
    }
}
