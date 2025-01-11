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
        public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime dateTimeAppointment)
        {
            return !await _appointmentRepository.HasConflictAsync(doctorId, dateTimeAppointment);
        }
    }

}
