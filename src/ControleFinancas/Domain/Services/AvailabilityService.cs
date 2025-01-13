using AppointmentsManager.Domain.Entities;

namespace AppointmentsManager.Domain.Services
{
    public class AvailabilityService
    {
        public bool IsDoctorAvailable(Doctor doctor, DateTime appointmentDateTime)
        {
            if (!doctor.IsWithinWorkHours(appointmentDateTime))
            {
                return false;
            }

            if (doctor.HasConflict(appointmentDateTime))
            {
                return false;
            }
            return true; 
        }
    }

}
