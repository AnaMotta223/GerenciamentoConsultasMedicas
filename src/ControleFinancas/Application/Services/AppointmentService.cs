using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Exceptions;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Domain.Services;

namespace AppointmentsManager.Application.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly AvailabilityService _availabilityService;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            AvailabilityService availabilityService,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _availabilityService = availabilityService;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }
        private async Task<AppointmentResponseDTO> MapToAppointmentResponseDTOAsync(Appointment appointment)
        {
            var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId)
            ?? throw new ArgumentException("Médico não encontrado.");
            var patient = await _patientRepository.GetByIdAsync(appointment.PatientId)
            ?? throw new ArgumentException("Paciente não encontrado.");

            return new AppointmentResponseDTO
            {
                Id = appointment.Id,
                DoctorName = doctor.Name,
                DoctorLastName = doctor.LastName,
                PatientName = patient.Name,
                PatientLastName = patient.LastName,
                DateTimeAppointment = appointment.DateTimeAppointment.ToString("dd/MM/yyyy HH:mm"),
                AppointmentStatus = appointment.AppointmentStatus,
                Notes = appointment.Notes
            };
        }
        public async Task<AppointmentResponseDTO> SearchAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"Nenhuma consulta encontrada com o ID {id}.");

            return await MapToAppointmentResponseDTOAsync(appointment);
        }
        public async Task<IEnumerable<AppointmentResponseDTO>> SearchAppointmentsByDateAsync(DateTime dateTime)
        {
            var appointments = await _appointmentRepository.GetByDateAsync(dateTime);

            if (appointments == null || !appointments.Any())
            {
                throw new KeyNotFoundException("Não há consultas cadastradas para essa data.");
            }

            var appointmentDtos = await Task.WhenAll(appointments.Select(MapToAppointmentResponseDTOAsync));

            return appointmentDtos;
        }
        public async Task<IEnumerable<AppointmentResponseDTO>> ListAppointmentsAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return await Task.WhenAll(appointments.Select(MapToAppointmentResponseDTOAsync));
        }
        public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDateTime)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId)
                         ?? throw new ArgumentException("Médico não encontrado.");

            var appointments = await _appointmentRepository.GetAllAsync();

            var doctorAppointments = appointments.Where(a => a.DoctorId == doctorId).ToList();

            var schedule = await _doctorRepository.GetDateTimeWork(doctorId);

            var doctorSchedule = schedule.Select(MapToDoctorDateTimeWorkResponseDTO).ToList();

            return _availabilityService.IsDoctorAvailable(doctor, appointmentDateTime, doctorSchedule, doctorAppointments);
        }
        public async Task<AppointmentResponseDTO> ScheduleAppointmentAsync(CreateAppointmentDTO createAppointmentDTO)
        {
            var doctor = await _doctorRepository.GetByIdAsync(createAppointmentDTO.DoctorId)
                         ?? throw new ArgumentException("Médico não encontrado.");
            var patient = await _patientRepository.GetByIdAsync(createAppointmentDTO.PatientId)
                         ?? throw new ArgumentException("Paciente não encontrado.");

            var dateTimeAppointment = createAppointmentDTO.DateTimeAppointment.ToDateTime();

            if (dateTimeAppointment < DateTime.Now)
            {
                throw new InvalidDateTimeException("A data de agendamento não pode ser no passado ou no dia atual.");
            }
            if (!IsValidAppointmentTime(dateTimeAppointment))
            {
                throw new InvalidDateTimeException("As consultas devem ser marcadas a cada 30 minutos.");
            }

            var appointments = await _appointmentRepository.GetAllAsync();
            var doctorAppointments = appointments.Where(a => a.DoctorId == createAppointmentDTO.DoctorId).ToList();
            var schedule = await _doctorRepository.GetDateTimeWork(createAppointmentDTO.DoctorId);
            var doctorSchedule = schedule.Select(MapToDoctorDateTimeWorkResponseDTO).ToList();

           
            if (!_availabilityService.IsDoctorAvailable(doctor, dateTimeAppointment, doctorSchedule, doctorAppointments))
            {
                throw new InvalidDateTimeException("O médico não está disponível nesta data e/ou horário.");
            }

            var appointment = new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                DateTimeAppointment = dateTimeAppointment,
                AppointmentStatus = AppointmentStatus.SCHEDULED,
                Notes = string.Empty
            };
            doctor.Appointments.Add(appointment);
            await _appointmentRepository.AddAsync(appointment);

            return await MapToAppointmentResponseDTOAsync(appointment);
        }
        public async Task<AppointmentResponseDTO> UpdateAppointmentAsync(int id, UpdateAppointmentDTO updateAppointmentDTO)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id)
                                 ?? throw new ArgumentException("Consulta não encontrada.");

            var dateTimeAppointment = updateAppointmentDTO.DateTimeAppointment.ToDateTime();

            if (dateTimeAppointment < DateTime.Now)
            {
                throw new InvalidDateTimeException("A data de agendamento não pode ser no passado ou no dia atual.");
            }
            if (!IsValidAppointmentTime(dateTimeAppointment))
            {
                throw new InvalidDateTimeException("As consultas devem ser marcadas a cada 30 minutos.");
            }

            var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);
            var appointments = await _appointmentRepository.GetAllAsync();
            var doctorAppointments = appointments.Where(a => a.DoctorId == appointment.DoctorId).ToList();
            var schedule = await _doctorRepository.GetDateTimeWork(appointment.DoctorId);
            var doctorSchedule = schedule.Select(MapToDoctorDateTimeWorkResponseDTO).ToList();

            if (!_availabilityService.IsDoctorAvailable(doctor, dateTimeAppointment, doctorSchedule, doctorAppointments))
            {
                throw new InvalidDateTimeException("O médico não está disponível neste horário.");
            }

            appointment.DateTimeAppointment = dateTimeAppointment;
            appointment.AppointmentStatus = updateAppointmentDTO.AppointmentStatus;
            appointment.Notes = updateAppointmentDTO.Notes;

            await _appointmentRepository.UpdateAsync(appointment.Id, appointment.DateTimeAppointment, appointment.AppointmentStatus, appointment.Notes);

            return await MapToAppointmentResponseDTOAsync(appointment);
        }
        public async Task<AppointmentResponseDTO> UpdateAppointmentStatusAsync(int id, UpdateStatusAppointmentDTO updateStatusAppointmentDTO)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id)
                         ?? throw new ArgumentException("Consulta não encontrada.");
            if (((int)updateStatusAppointmentDTO.Status) < 0 || ((int)updateStatusAppointmentDTO.Status) > 3)
            {
                throw new InvalidEnumNumberException("Status inválido. Valor não encontrado.");
            }
            appointment.AppointmentStatus = updateStatusAppointmentDTO.Status;
            appointment.Notes = updateStatusAppointmentDTO.Notes;

            await _appointmentRepository.UpdateAsync(appointment.Id, appointment.DateTimeAppointment, appointment.AppointmentStatus, appointment.Notes);

            return await MapToAppointmentResponseDTOAsync(appointment);
        }
        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id)
                         ?? throw new ArgumentException("Consulta não encontrada.");

            if (appointment.AppointmentStatus != AppointmentStatus.CANCELED)
            {
                throw new InvalidOperationException("Consultas agendadas ou confirmadas não podem ser deletadas.");
            }
            await _appointmentRepository.DeleteAsync(id);
        }
        private static DoctorDateTimeWorkResponseDTO MapToDoctorDateTimeWorkResponseDTO(DateTimeWork dateTimeWork)
        {
            return new DoctorDateTimeWorkResponseDTO
            {
                DayOfWeek = dateTimeWork.DayOfWeek,
                StartTime = dateTimeWork.StartTime.ToString(@"hh\:mm\:ss"),
                EndTime = dateTimeWork.EndTime.ToString(@"hh\:mm\:ss")
            };
        }
        private bool IsValidAppointmentTime(DateTime appointmentTime)
        {
            return appointmentTime.Minute == 0 || appointmentTime.Minute == 30;
        }
    }
}
