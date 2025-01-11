using AppointmentsManager.Application.Services;
using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Interfaces;

namespace AppointmentsManager.Domain.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly AvailabilityService _availabilityService;

        public AppointmentService(IAppointmentRepository appointmentRepository, AvailabilityService availabilityService)
        {
            _appointmentRepository = appointmentRepository;
            _availabilityService = availabilityService;
        }
        //Colocar os outros metodos (editar, excluir etc)

        public Appointment ScheduleAppointment(Doctor doctor, DateTime appointmentDate)
        {
            if (!_availabilityService.IsDoctorAvailable(doctor.Id, appointmentDate))
            {
                throw new InvalidOperationException("O médico não está disponível neste horário.");
            }

            // Cria a consulta
            var appointment = new Appointment
            {
                DoctorId = doctor.Id,
                DateTimeAppointment = appointmentDate,
                PatientId = patient.Id,
                AppointmentStatus = AppointmentStatus.Scheduled
            };

            _appointmentRepository.Add(appointment);

            return appointment;
        }
    }

}
