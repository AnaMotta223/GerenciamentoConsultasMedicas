using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Exceptions;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Domain.Services;
using AppointmentsManager.Infrastructure.Repositories;

namespace AppointmentsManager.Application.Services
{
    public class DateTimeWorkService
    {
        private readonly IDateTimeWorkRepository _dateTimeWorkRepository;
        private readonly AvailabilityService _availabilityService;
        private readonly IDoctorRepository _doctorRepository;
        public DateTimeWorkService(IDateTimeWorkRepository dateTimeWorkRepository, AvailabilityService availabilityService, IDoctorRepository doctorRepository)
        {
            _dateTimeWorkRepository = dateTimeWorkRepository;
            _availabilityService = availabilityService;
            _doctorRepository = doctorRepository;
        }
        public async Task<IEnumerable<DateTimeWork>> ListPatientsAsync()
        {
            return await _dateTimeWorkRepository.GetAllAsync();
        }
        public async Task<DateTimeWorkResponseDTO> SearchScheduleAsync(int id)
        {
            var doctor = await _dateTimeWorkRepository.GetByIdAsync(id)
                          ?? throw new KeyNotFoundException($"Nenhum horário de trabalho encontrado com o ID {id}.");
            var dateTimeWorkDTO = new DateTimeWorkResponseDTO
            {
                Id = id,
                DayOfWeek = doctor.DayOfWeek,
                StartTime = doctor.StartTime.ToString(),
                EndTime = doctor.EndTime.ToString()
            };
            return dateTimeWorkDTO;
        }
        public async Task<DateTimeWorkResponseDTO> CreateScheduleAsync(CreateDateTimeWorkDTO createDateTimeWorkDTO)
        {
            var doctor = await _doctorRepository.GetByIdAsync(createDateTimeWorkDTO.IdDoctor)
                         ?? throw new ArgumentException("Médico não encontrado.");

            if (!_availabilityService.IsDateTimeValid(createDateTimeWorkDTO.DayOfWeek, createDateTimeWorkDTO.StartTime.ToTimeSpan(), createDateTimeWorkDTO.EndTime.ToTimeSpan()))
            {
                throw new InvalidDateTimeException("Dia ou horário inválidos.");
            }

            var dateTimeWork = new DateTimeWork
            {
                IdDoctor = createDateTimeWorkDTO.IdDoctor,
                DayOfWeek = createDateTimeWorkDTO.DayOfWeek,
                StartTime = createDateTimeWorkDTO.StartTime.ToTimeSpan(),
                EndTime = createDateTimeWorkDTO.EndTime.ToTimeSpan()
            };

            await _dateTimeWorkRepository.AddAsync(dateTimeWork);

            var dateTimeWorkDTO = new DateTimeWorkResponseDTO
            {
                DayOfWeek = createDateTimeWorkDTO.DayOfWeek,
                StartTime = DateTime.Today.Add(createDateTimeWorkDTO.StartTime.ToTimeSpan()).ToString("dd/MM/yyyy HH:mm"),
                EndTime = DateTime.Today.Add(createDateTimeWorkDTO.EndTime.ToTimeSpan()).ToString("dd/MM/yyyy HH:mm")
            };

            return dateTimeWorkDTO;
        }

    }
}
