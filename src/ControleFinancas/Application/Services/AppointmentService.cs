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

        public AppointmentService(IAppointmentRepository appointmentRepository, AvailabilityService availabilityService, IDoctorRepository doctorRepository, IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _availabilityService = availabilityService;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }
        public async Task<AppointmentResponseDTO> SearchAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"Nenhuma consulta encontrada com o ID {id}.");
            }

            return new AppointmentResponseDTO
            {
                Id = appointment.Id,
                DoctorId = appointment.Doctor.Id,
                PatientId = appointment.Patient.Id,
                DateTimeAppointment = appointment.DateTimeAppointment.ToString("dd/MM/yyyy HH:mm:ss"),
                AppointmentStatus = appointment.AppointmentStatus,
                Notes = appointment.Notes
            };
        }
        public async Task<IEnumerable<AppointmentResponseDTO>> ListAppointmentsAsync()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAllAsync();

                return appointments?.Select(appointment => new AppointmentResponseDTO
                {
                    Id = appointment.Id,
                    DoctorId = appointment.Doctor.Id,
                    PatientId = appointment.Patient.Id,
                    DateTimeAppointment = appointment.DateTimeAppointment.ToString("dd/MM/yyyy HH:mm:ss"),
                    AppointmentStatus = appointment.AppointmentStatus,
                    Notes = appointment.Notes
                }) ?? new List<AppointmentResponseDTO>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ocorreu um erro ao listar as consultas.", ex);
            }
        }
        public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDateTime)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
            {
                throw new ArgumentException("Médico não encontrado.");
            }

            return _availabilityService.IsDoctorAvailable(doctor, appointmentDateTime);
        }
        public async Task<AppointmentResponseDTO> ScheduleAppointmentAsync(CreateAppointmentDTO createAppointmentDTO)
        {
            var doctor = await _doctorRepository.GetByIdAsync(createAppointmentDTO.DoctorId);
            var patient = await _patientRepository.GetByIdAsync(createAppointmentDTO.PatientId);
            var isAvailable = _availabilityService.IsDoctorAvailable(doctor, createAppointmentDTO.DateTimeAppointment);

            if (!isAvailable)
            {
                throw new InvalidDateTimeException("O médico não está disponível neste horário.");
            }
            if (doctor == null)
            {
                throw new ArgumentException("Médico não encontrado.");
            }
            if (patient == null)
            {
                throw new ArgumentException("Paciente não encontrado.");
            }
            if (createAppointmentDTO.DateTimeAppointment < DateTime.Now)
            {
                throw new InvalidDateTimeException("A data de agendamento não pode ser no passado.");
            }

            var appointment = new Appointment
            {
                Doctor = doctor,
                Patient = patient,
                DateTimeAppointment = createAppointmentDTO.DateTimeAppointment,
                AppointmentStatus = AppointmentStatus.SCHEDULED,
                Notes = string.Empty
            };

            await _appointmentRepository.AddAsync(appointment);

            var appointmentDto = new AppointmentResponseDTO
            {
                Id = appointment.Id,
                DoctorId = appointment.Doctor.Id,
                PatientId = appointment.Patient.Id,
                DateTimeAppointment = appointment.DateTimeAppointment.ToString("dd/MM/yyyy HH:mm:ss"),
                AppointmentStatus = appointment.AppointmentStatus,
                Notes = appointment.Notes
            };

            return appointmentDto;
        }
        public async Task<AppointmentResponseDTO> UpdateAppointmentAsync(int id, UpdateAppointmentDTO updateAppointmentDTO)
        {
            //verificar se o id do usuario logado bate com o id do medico especialmente pro dia atual
            //verificar se o id do usuario bate com o id do paciente - restringir datas 

            var appointment = await _appointmentRepository.GetByIdAsync(id);
            var isAvailable = _availabilityService.IsDoctorAvailable(appointment.Doctor, updateAppointmentDTO.DateTimeAppointment);
            if(appointment == null)
            {
                throw new ArgumentException("Consulta não encontrada.");
            }
            if (!isAvailable)
            {
                throw new InvalidDateTimeException("O médico não está disponível neste horário.");
            }
            if (updateAppointmentDTO.DateTimeAppointment < DateTime.Now)
            {
                throw new InvalidDateTimeException("A data de agendamento não pode ser no passado.");
            }
            if (string.IsNullOrWhiteSpace(appointment.Notes))
            {
                appointment.AppointmentStatus = updateAppointmentDTO.AppointmentStatus;
                appointment.DateTimeAppointment = updateAppointmentDTO.DateTimeAppointment;
                await _appointmentRepository.UpdateAsync(id, appointment.DateTimeAppointment, appointment.AppointmentStatus);
                return new AppointmentResponseDTO
                {
                    Id = appointment.Id,
                    DoctorId = appointment.Doctor.Id,
                    PatientId = appointment.Patient.Id,
                    DateTimeAppointment = updateAppointmentDTO.DateTimeAppointment.ToString("dd/MM/yyyy HH:mm:ss"),
                    AppointmentStatus = updateAppointmentDTO.AppointmentStatus,
                    Notes = appointment.Notes
                };
            }
            appointment.AppointmentStatus = updateAppointmentDTO.AppointmentStatus;
            appointment.DateTimeAppointment = updateAppointmentDTO.DateTimeAppointment;
            appointment.Notes = updateAppointmentDTO.Notes;
            await _appointmentRepository.UpdateAsync(id, appointment.DateTimeAppointment, appointment.AppointmentStatus, appointment.Notes);
            return new AppointmentResponseDTO
            {
                Id = appointment.Id,
                DoctorId = appointment.Doctor.Id,
                PatientId = appointment.Patient.Id,
                DateTimeAppointment = updateAppointmentDTO.DateTimeAppointment.ToString("dd/MM/yyyy HH:mm:ss"),
                AppointmentStatus = updateAppointmentDTO.AppointmentStatus,
                Notes = updateAppointmentDTO.Notes
            };
        }
        public async Task<AppointmentResponseDTO> UpdateAppointmentStatusAsync(int id, UpdateStatusAppointmentDTO updateStatusAppointmentDTO)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                throw new ArgumentException("Consulta não encontrada.");
            }
            if (string.IsNullOrWhiteSpace(appointment.Notes))
            {
                appointment.AppointmentStatus = updateStatusAppointmentDTO.Status;
                await _appointmentRepository.UpdateStatusAsync(id, appointment.AppointmentStatus);
                return new AppointmentResponseDTO
                {
                    Id = appointment.Id,
                    DoctorId = appointment.Doctor.Id,
                    PatientId = appointment.Patient.Id,
                    DateTimeAppointment = appointment.DateTimeAppointment.ToString("dd/MM/yyyy HH:mm:ss"),
                    AppointmentStatus = updateStatusAppointmentDTO.Status,
                    Notes = appointment.Notes
                };
            }
            appointment.AppointmentStatus = updateStatusAppointmentDTO.Status;
            appointment.Notes = updateStatusAppointmentDTO.Notes;
            await _appointmentRepository.UpdateStatusAsync(id, appointment.AppointmentStatus, appointment.Notes);
            return new AppointmentResponseDTO
            {
                Id = appointment.Id,
                DoctorId = appointment.Doctor.Id,
                PatientId = appointment.Patient.Id,
                DateTimeAppointment = appointment.DateTimeAppointment.ToString("dd/MM/yyyy HH:mm:ss"),
                AppointmentStatus = updateStatusAppointmentDTO.Status,
                Notes = updateStatusAppointmentDTO.Notes
            };
        }
        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                throw new ArgumentException("Consulta não encontrada.");
            }
            if (appointment.AppointmentStatus != AppointmentStatus.CANCELED)
            {
                throw new InvalidOperationException("Consultas agendadas ou confirmadas não podem ser deletadas.");
            }
            await _appointmentRepository.DeleteAsync(id);
        }
    }
}
