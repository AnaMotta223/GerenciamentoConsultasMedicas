using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Exceptions;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Utils;

namespace AppointmentsManager.Application.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly PasswordEncrypter _passwordEncrypter;

        public DoctorService(IDoctorRepository doctorRepository, PasswordEncrypter passwordEncrypter)
        {
            _doctorRepository = doctorRepository;
            _passwordEncrypter = passwordEncrypter;
        }
        public async Task<IEnumerable<DoctorResponseDTO>> ListDoctorsAsync()
        {
            try
            {
                var doctors = await _doctorRepository.GetAllAsync();

                return doctors?.Select(doctor => new DoctorResponseDTO
                {
                    Id = doctor.Id,
                    Name = doctor.Name,
                    LastName = doctor.LastName,
                    Email = doctor.Email,
                    Phone = doctor.Phone,
                    Address = doctor.Address,
                    BirthDate = doctor.BirthDate.ToString("dd/MM/yyyy"),
                    Gender = doctor.Gender,
                    Speciality = doctor.Speciality,
                    RMC = doctor.RMC,
                }) ?? new List<DoctorResponseDTO>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ocorreu um erro ao listar os médicos.", ex);
            }
        }
        public async Task<DoctorResponseDTO> SearchDoctorAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"Nenhum médico encontrado com o ID {id}.");
            }

            return new DoctorResponseDTO
            {
                Id = doctor.Id,
                Name = doctor.Name,
                LastName = doctor.LastName,
                Email = doctor.Email,
                Phone = doctor.Phone,
                Address = doctor.Address,
                BirthDate = doctor.BirthDate.ToString("dd/MM/yyyy"),
                Gender = doctor.Gender,
                Speciality = doctor.Speciality,
                RMC = doctor.RMC
            };
        }
        public async Task<IEnumerable<DoctorScheduleResponseDTO>> GetDoctorScheduleAsync(int id)
        {
            var schedule = await _doctorRepository.GetDateTimeWork(id);

            if(schedule == null)
            {
                throw new ArgumentNullException("Horários de trabalho ainda não informados ou médico de férias.");
            }
            var dateTimeWorkList = schedule.Select(dto => new DoctorScheduleResponseDTO
            {
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime.ToString(@"hh\:mm\:ss"),
                EndTime = dto.EndTime.ToString(@"hh\:mm\:ss")
            }).ToList();

            return dateTimeWorkList;
        }
        public async Task<DoctorResponseDTO> RegisterDoctorAsync(CreateDoctorDTO createDoctorDTO)
        {
            var doctors = await _doctorRepository.GetAllAsync();

            if (createDoctorDTO.BirthDate >= DateTime.Now || (DateTime.Now.Year - createDoctorDTO.BirthDate.Year < 18))
            {
                throw new InvalidDateTimeException("Data de nascimento inválida.");
            }
            if (doctors.Any(a => a.CPF.Equals(createDoctorDTO.CPF)))
            {
                throw new InvalidCPFException("CPF já cadastrado.");
            }
            if (doctors.Any(a => a.Email.Equals(createDoctorDTO.Email)))
            {
                throw new InvalidEmailException("Email já cadastrado.");
            }
            if (doctors.Any(a => a.RMC == createDoctorDTO.RMC))
            {
                throw new InvalidRMCException("CRM já cadastrado.");
            }
            var doctor = new Doctor
            {
                Name = createDoctorDTO.Name,
                LastName = createDoctorDTO.LastName,
                Email = createDoctorDTO.Email,
                Password = _passwordEncrypter.HashPassword(createDoctorDTO.Password),
                Phone = createDoctorDTO.Phone,
                Address = createDoctorDTO.Address,
                BirthDate = createDoctorDTO.BirthDate,
                Gender = createDoctorDTO.Gender,
                CPF = createDoctorDTO.CPF,
                Speciality = createDoctorDTO.Speciality,
                RMC = createDoctorDTO.RMC
            };

            await _doctorRepository.AddAsync(doctor);

            var doctorDto = new DoctorResponseDTO
            {
                Id = doctor.Id,
                Name = doctor.Name,
                LastName = doctor.LastName,
                Email = doctor.Email,
                Phone = doctor.Phone,
                Address = doctor.Address,
                BirthDate = doctor.BirthDate.ToString("dd/MM/yyyy"),
                Gender = doctor.Gender,
                Speciality = doctor.Speciality,
                RMC = doctor.RMC
            };

            return doctorDto;
        }
        public async Task<DoctorResponseDTO> UpdateDoctorAsync(int id, UpdateDoctorDTO updateDoctorDTO)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            var doctors = await _doctorRepository.GetAllAsync();

            if (doctor == null)
            {
                throw new ArgumentException("Médico não encontrado.");
            }
            if (updateDoctorDTO.BirthDate >= DateTime.Now || (DateTime.Now.Year - updateDoctorDTO.BirthDate.Year < 18))
            {
                throw new InvalidDateTimeException("Data de nascimento inválida.");
            }
            if (doctors.Any(a => a.Email.Equals(updateDoctorDTO.Email)))
            {
                throw new InvalidEmailException("Email já cadastrado.");
            }
            if (doctors.Any(a => a.RMC == updateDoctorDTO.RMC))
            {
                throw new InvalidRMCException("CRM já cadastrado.");
            }
            await _doctorRepository.UpdateAsync(id, updateDoctorDTO.Name,
                updateDoctorDTO.LastName,
                _passwordEncrypter.HashPassword(updateDoctorDTO.Password),
                updateDoctorDTO.Address,
                updateDoctorDTO.BirthDate,
                updateDoctorDTO.Gender,
                updateDoctorDTO.Email,
                updateDoctorDTO.Phone,
                updateDoctorDTO.RMC,
                updateDoctorDTO.Speciality);

            return new DoctorResponseDTO
            {
                Id = doctor.Id,
                Name = doctor.Name,
                LastName = doctor.LastName,
                Email = doctor.Email,
                Phone = doctor.Phone,
                Address = doctor.Address,
                BirthDate = doctor.BirthDate.ToString("dd/MM/yyyy"),
                Gender = doctor.Gender,
                Speciality = doctor.Speciality,
                RMC = doctor.RMC
            };
        }
        public async Task<List<DoctorScheduleResponseDTO>> UpdateDoctorScheduleAsync(int id, List<UpdateDoctorScheduleDTO> updateDoctorScheduleDTO)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new ArgumentException("Médico não encontrado.");
            }
            var dateTimeWorkList = updateDoctorScheduleDTO.Select(dto => new DateTimeWork
            {   
                Doctor = doctor,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            }).ToList();
            await _doctorRepository.UpdateDateTimeWorkAsync(id, dateTimeWorkList);

            var dateTimeWorkListDto = updateDoctorScheduleDTO.Select(dto => new DoctorScheduleResponseDTO
            {
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime.ToString(@"hh\:mm\:ss"),
                EndTime = dto.EndTime.ToString(@"hh\:mm\:ss")
            }).ToList();

            return dateTimeWorkListDto;
        }
        public async Task DeleteDoctorAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new ArgumentException("Médico não encontrado.");
            }
            await _doctorRepository.DeleteAsync(id);
        }
    }
}
