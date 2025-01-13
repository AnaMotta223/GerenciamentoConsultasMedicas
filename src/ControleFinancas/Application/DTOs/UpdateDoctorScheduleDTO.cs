namespace AppointmentsManager.Application.DTOs
{
    public class UpdateDoctorScheduleDTO
    {
        public int DayOfWeek { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
