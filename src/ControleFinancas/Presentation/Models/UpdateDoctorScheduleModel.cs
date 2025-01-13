namespace AppointmentsManager.Presentation.Models
{
    public class UpdateDoctorScheduleModel
    {
        public int DayOfWeek { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
