using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Entities;

namespace AppointmentsManager.Domain.Services
{
    public class AvailabilityService
    {
        public bool IsDoctorAvailable(Doctor doctor, DateTime appointmentDateTime, List<DoctorDateTimeWorkResponseDTO> dateTimeWorkList, List<Appointment> appointments)
        {
            if (!doctor.IsWithinWorkHours(appointmentDateTime, dateTimeWorkList))
            {
                return false;
            }

            if (doctor.HasConflict(appointmentDateTime, appointments))
            {
                return false;
            }
            return true; 
        }
        public bool IsDateTimeValid(int dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            if (dayOfWeek < 1 || dayOfWeek > 7)
            {
                return false;
            }
            if (startTime >= endTime)
            {
                return false;
            }
            return true;
        }
    }

}
