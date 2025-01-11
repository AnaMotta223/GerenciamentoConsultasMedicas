using AppointmentsManager.Application.DTOs;
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
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, AvailabilityService availabilityService, IDoctorRepository doctorRepository, IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _availabilityService = availabilityService;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }
        public async Task<Appointment> SearchAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"Nenhuma consulta encontrada com o ID {id}.");
            }

            return appointment;
        }
        public async Task<IEnumerable<Appointment>> ListAppointmentsAsync()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAllAsync();

                return appointments ?? new List<Appointment>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ocorreu um erro ao listar as consultas.", ex);
            }
        }

        public async Task<Appointment> ScheduleAppointmentAsync(CreateAppointmentDTO createAppointmentDTO)
        {
            var isAvailable = await _availabilityService.IsDoctorAvailableAsync(createAppointmentDTO.DoctorId, createAppointmentDTO.DateTimeAppointment);
            if (!isAvailable)
            {
                throw new InvalidOperationException("O médico não está disponível neste horário.");
            }

            var doctor = await _doctorRepository.GetByIdAsync(createAppointmentDTO.DoctorId);
            if (doctor == null)
            {
                throw new ArgumentException("Médico não encontrado.");
            }

            var patient = await _patientRepository.GetByIdAsync(createAppointmentDTO.PatientId);
            if (patient == null)
            {
                throw new ArgumentException("Paciente não encontrado.");
            }

            var appointment = new Appointment
            {
                Doctor = doctor,
                Patient = patient,
                DateTimeAppointment = createAppointmentDTO.DateTimeAppointment,
                AppointmentStatus = AppointmentStatus.Scheduled,
                Notes = string.Empty 
            };

            await _appointmentRepository.AddAsync(appointment);

            return appointment;
        }
        public async Task<Appointment> UpdateAppointmentAsync(int id, UpdateAppointmentDTO updateAppointmentDTO)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (string.IsNullOrWhiteSpace(appointment.Notes))
            {
                appointment.AppointmentStatus = updateAppointmentDTO.AppointmentStatus;
                appointment.DateTimeAppointment = updateAppointmentDTO.DateTimeAppointment;
                await _appointmentRepository.UpdateAsync(id,appointment.DateTimeAppointment, appointment.AppointmentStatus);
                return appointment;
            }
            appointment.AppointmentStatus = updateAppointmentDTO.AppointmentStatus;
            appointment.DateTimeAppointment = updateAppointmentDTO.DateTimeAppointment;
            appointment.Notes = updateAppointmentDTO.Notes;
            await _appointmentRepository.UpdateAsync(id, appointment.DateTimeAppointment, appointment.AppointmentStatus, appointment.Notes);
            return appointment;

        }

    }


}
