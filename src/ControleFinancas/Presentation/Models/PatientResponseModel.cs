using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Presentation.Models
{
    public class PatientResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}
