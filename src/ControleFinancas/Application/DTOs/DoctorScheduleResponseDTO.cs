namespace AppointmentsManager.Application.DTOs
{
    public class DoctorScheduleResponseDTO
    {
        public int DayOfWeek { get; set; } 
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
