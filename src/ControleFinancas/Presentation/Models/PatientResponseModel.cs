using AppointmentsManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppointmentsManager.Presentation.Models
{
    public class PatientResponseModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = "O formato da data deve ser dd/MM/yyyy.")]
        public string BirthDate { get; set; }
        public string Gender { get; set; }
    }
}
