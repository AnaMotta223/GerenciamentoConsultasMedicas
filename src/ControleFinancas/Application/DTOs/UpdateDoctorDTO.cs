using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Application.DTOs
{
    public class UpdateDoctorDTO
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
    }
}
