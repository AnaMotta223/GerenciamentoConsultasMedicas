using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AppointmentsManager.Application.DTOs
{
    public class UpdateDoctorScheduleDTO
    {
        public int DayOfWeek { get; set; }

        [SwaggerSchema(Format = "time", Description = "Hora de início no formato HH:mm")]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "O formato do horário deve ser HH:mm.")]
        public string StartTime { get; set; }

        [SwaggerSchema(Format = "time", Description = "Hora de término no formato HH:mm")]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "O formato do horário deve ser HH:mm.")]
        public string EndTime { get; set; }

        public UpdateDoctorScheduleDTO() { }
        public UpdateDoctorScheduleDTO(int dayOfWeek, string startTime, string endTime)
        {
            if (dayOfWeek < 1 || dayOfWeek > 7)
                throw new ArgumentException("O dia da semana deve estar entre 1 e 7.");

            if (startTime.ToTimeSpan() >= endTime.ToTimeSpan())
                throw new ArgumentException("O horário de início deve ser anterior ao horário de fim.");

            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }
        public string GetDayName()
        {
            return DayOfWeek switch
            {
                1 => "Domingo",
                2 => "Segunda-feira",
                3 => "Terça-feira",
                4 => "Quarta-feira",
                5 => "Quinta-feira",
                6 => "Sexta-feira",
                7 => "Sábado",
                _ => throw new ArgumentException("Dia da semana inválido.")
            };
        }
    }
}

