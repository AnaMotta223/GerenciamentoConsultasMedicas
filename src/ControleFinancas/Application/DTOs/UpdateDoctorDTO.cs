using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AppointmentsManager.Application.DTOs
{
    public class UpdateDoctorDTO
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
    }
}
