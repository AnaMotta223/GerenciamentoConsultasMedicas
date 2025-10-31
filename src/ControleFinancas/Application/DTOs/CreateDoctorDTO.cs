using AppointmentsManager.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace AppointmentsManager.Application.DTOs
{
    public class CreateDoctorDTO
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [SwaggerSchema(Format = "date", Description = "Data de nascimento no formato dd/MM/yyyy")]
        public string BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string CPF { get; set; }
        public Speciality Speciality { get; set; }
        public string RMC { get; set; }
        public List<DoctorDateTimeWorkResponseDTO> DateTimeWorkList { get; set; }
    }
}
