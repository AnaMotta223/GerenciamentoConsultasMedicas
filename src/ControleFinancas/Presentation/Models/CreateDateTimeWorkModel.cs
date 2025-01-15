using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AppointmentsManager.Application.DTOs
{
    public class CreateDateTimeWorkModel
    {
        public int IdDoctor { get; set; }
        public int DayOfWeek { get; set; } 

        [SwaggerSchema(Format = "time", Description = "Hora de início no formato HH:mm")]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "O formato do horário deve ser HH:mm.")]
        public string StartTime { get; set; }

        [SwaggerSchema(Format = "time", Description = "Hora de término no formato HH:mm")]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "O formato do horário deve ser HH:mm.")]
        public string EndTime { get; set; }
        public CreateDateTimeWorkDTO ToDto()
        {
            return new CreateDateTimeWorkDTO
            {
                IdDoctor = IdDoctor,
                DayOfWeek = this.DayOfWeek,
                StartTime = this.StartTime,
                EndTime = this.EndTime
            };
        }
    }
}
