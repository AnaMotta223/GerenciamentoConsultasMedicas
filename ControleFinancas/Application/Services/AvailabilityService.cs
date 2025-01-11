using AppointmentsManager.Domain.Interfaces;

namespace AppointmentsManager.Application.Services
{
    public class AvailabilityService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AvailabilityService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public bool IsDoctorAvailable(int id, DateTime dateTimeAppointment)
        {
            return !_appointmentRepository.HasConflict(id, dateTimeAppointment);
        }
    }

}
