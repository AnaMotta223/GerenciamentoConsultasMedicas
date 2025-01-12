using AppointmentsManager.Domain.Entities;

namespace AppointmentsManager.Domain.Services
{
    public class AvailabilityService
    {
        public bool IsDoctorAvailable(Doctor doctor, DateTime appointmentDateTime)
        {
            // Verifica se está dentro do horário de trabalho
            if (!doctor.IsWithinWorkHours(appointmentDateTime))
            {
                return false;
            }

            // Verifica se já há uma consulta no horário
            if (doctor.HasConflict(appointmentDateTime))
            {
                return false;
            }
            return true; // Disponível
        }
    }

}
