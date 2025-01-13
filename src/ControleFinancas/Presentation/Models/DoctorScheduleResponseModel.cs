namespace AppointmentsManager.Presentation.Models
{
    public class DoctorScheduleResponseModel
    {
        public int DayOfWeek { get; set; } 
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
