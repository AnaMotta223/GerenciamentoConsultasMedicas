using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Exceptions;
using AppointmentsManager.Domain.Interfaces;                       
using AppointmentsManager.Utils;

namespace AppointmentsManager.Application.Services
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly PasswordEncrypter _passwordEncrypter;
        private readonly IDateTimeWorkRepository _dateTimeWorkRepository;

        public DoctorService(IDoctorRepository doctorRepository, PasswordEncrypter passwordEncrypter, IDateTimeWorkRepository dateTimeWorkRepository, IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _passwordEncrypter = passwordEncrypter;
            _dateTimeWorkRepository = dateTimeWorkRepository;
        }
        public async Task<IEnumerable<DoctorResponseDTO>> ListDoctorsAsync()
        {
            var schedules = await _dateTimeWorkRepository.GetAllAsync();

            var doctors = await _doctorRepository.GetAllAsync();

            var schedulesByDoctorId = schedules
                .GroupBy(schedule => schedule.IdDoctor)
                .ToDictionary(group => group.Key, group => group.Select(MapToDoctorDateTimeWorkResponseDTO).ToList());

            var doctorDtos = doctors.Select(doctor =>
            {
                doctor.DateTimeWorkList = schedulesByDoctorId.TryGetValue(doctor.Id, out var doctorSchedules)
                    ? doctorSchedules
                    : new List<DoctorDateTimeWorkResponseDTO>();

                return MapToDoctorResponseDTO(doctor);
            });

            return doctorDtos;
        }
        public async Task<DoctorResponseDTO> SearchDoctorAsync(int id)
        {
            var schedule = await _doctorRepository.GetDateTimeWork(id);

            var doctor = await _doctorRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Nenhum médico encontrado com o ID {id}.");

            doctor.DateTimeWorkList = schedule
                .Select(MapToDoctorDateTimeWorkResponseDTO)
                .ToList();

            return MapToDoctorResponseDTO(doctor);
        }
        public async Task<DoctorResponseDTO> RegisterDoctorAsync(CreateDoctorDTO createDoctorDTO)
        {
            if (createDoctorDTO.BirthDate.ToDateTime() >= DateTime.Now)
            {
                throw new InvalidDateTimeException("Data de nascimento inválida: A data de nascimento não pode ser no futuro.");
            }
            if ((DateTime.Now.Year - createDoctorDTO.BirthDate.ToDateTime().Year) < 18 || (DateTime.Now.Year - createDoctorDTO.BirthDate.ToDateTime().Year) == 18 && (((DateTime.Now.Month == createDoctorDTO.BirthDate.ToDateTime().Month) && (DateTime.Now.Day < createDoctorDTO.BirthDate.ToDateTime().Day)) || DateTime.Now.Month < createDoctorDTO.BirthDate.ToDateTime().Month))
            {
                throw new InvalidDateTimeException("Data de nascimento inválida: A idade não pode ser menor que 18 anos");
            }
            if (((int)createDoctorDTO.Gender) < 0 || ((int)createDoctorDTO.Gender) > 3)
            {
                throw new InvalidEnumNumberException("Gênero inválido. Valor não encontrado.");
            }
            if (((int)createDoctorDTO.Speciality) < 0 || ((int)createDoctorDTO.Speciality) > 15)
            {
                throw new InvalidEnumNumberException("Especialidade inválida. Valor não encontrado.");
            }
            if (createDoctorDTO.Phone.Length < 8 || createDoctorDTO.Phone.Length > 11)
            {
                throw new InvalidPhoneException("Número de telefone inválido. Deve ter de 8 a 11 dígitos.");
            }
            if (await _doctorRepository.ExistsByCPFAsync(createDoctorDTO.CPF) || await _patientRepository.ExistsByCPFAsync(createDoctorDTO.CPF))
            {
                throw new InvalidCPFException("CPF já cadastrado.");
            }
            if (await _doctorRepository.ExistsByEmailAsync(createDoctorDTO.Email) || await _patientRepository.ExistsByEmailAsync(createDoctorDTO.Email))
            {
                throw new InvalidEmailException("Email já cadastrado.");
            }
            if (await _doctorRepository.ExistsByRMCAsync(createDoctorDTO.RMC))
            {
                throw new InvalidRMCException("CRM já cadastrado.");
            }

            var doctor = new Doctor
            {
                Status = UserStatus.ACTIVE,
                Name = createDoctorDTO.Name,
                LastName = createDoctorDTO.LastName,
                Email = createDoctorDTO.Email.ToEmail(),
                Password = _passwordEncrypter.HashPassword(createDoctorDTO.Password),
                Phone = createDoctorDTO.Phone,
                Address = createDoctorDTO.Address,
                BirthDate = createDoctorDTO.BirthDate.ToDateTime(),
                Gender = createDoctorDTO.Gender,
                CPF = createDoctorDTO.CPF.ToCPF(),
                RMC = createDoctorDTO.RMC.ToRMC(),
                Speciality = createDoctorDTO.Speciality,
                DateTimeWorkList = new List<DoctorDateTimeWorkResponseDTO>()
            };

            var createdDoctor = await _doctorRepository.AddAsync(doctor);

            foreach (var dateTimeWorkDTO in createDoctorDTO.DateTimeWorkList)
            {
                if (dateTimeWorkDTO.DayOfWeek <= 0 || dateTimeWorkDTO.DayOfWeek > 7)
                {
                    await _doctorRepository.DeleteAsync(createdDoctor.Id);
                    throw new InvalidDateTimeException("Dia da semana inválido.");
                }
                var dateTimeWork = MapToDateTimeWork(createdDoctor.Id, dateTimeWorkDTO);
                await _dateTimeWorkRepository.AddAsync(dateTimeWork);
                doctor.DateTimeWorkList.Add(dateTimeWorkDTO);
            }

            return MapToDoctorResponseDTO(doctor);
        }
        private DoctorResponseDTO MapToDoctorResponseDTO(Doctor doctor)
        {
            return new DoctorResponseDTO
            {
                Id = doctor.Id,
                Status = doctor.Status.ToString(),
                Name = doctor.Name,
                LastName = doctor.LastName,
                Email = doctor.Email.ToString(),
                Phone = doctor.Phone,
                Address = doctor.Address,
                BirthDate = doctor.BirthDate.ToString("dd/MM/yyyy"),
                Gender = doctor.Gender.ToString(),
                Speciality = doctor.Speciality.ToString(),
                RMC = doctor.RMC.Value,
                DateTimeWorkList = doctor.DateTimeWorkList ?? new List<DoctorDateTimeWorkResponseDTO>()
            };
        }
        private static DateTimeWork MapToDateTimeWork(int idDoctor, DoctorDateTimeWorkResponseDTO dateTimeWorkDTO)
        {
            return new DateTimeWork
            {
                IdDoctor = idDoctor,
                DayOfWeek = dateTimeWorkDTO.DayOfWeek,
                StartTime = dateTimeWorkDTO.StartTime.ToTimeSpan(),
                EndTime = dateTimeWorkDTO.EndTime.ToTimeSpan()
            };
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
        public async Task<IEnumerable<DoctorScheduleResponseDTO>> GetDoctorScheduleAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Nenhum médico encontrado com o ID {id}.");
            var schedule = await _doctorRepository.GetDateTimeWork(id);

            if (schedule == null || !schedule.Any())
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
        public async Task<DoctorResponseDTO> UpdateDoctorAsync(int id, UpdateDoctorDTO updateDoctorDTO)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new ArgumentException("Médico não encontrado.");
            }
            if (updateDoctorDTO.Phone.Length < 8 || updateDoctorDTO.Phone.Length > 11)
            {
                throw new InvalidPhoneException("Número de telefone inválido. Deve ter de 8 a 11 dígitos.");
            }
            if (((int)updateDoctorDTO.Gender) < 0 || ((int)updateDoctorDTO.Gender) > 3)
            {
                throw new InvalidEnumNumberException("Gênero inválido. Valor não encontrado.");
            }
            if (((int)updateDoctorDTO.Speciality) < 0 || ((int)updateDoctorDTO.Speciality) > 15)
            {
                throw new InvalidEnumNumberException("Especialidade inválida. Valor não encontrado.");
            }
            if (updateDoctorDTO.BirthDate.ToDateTime() >= DateTime.Now)
            {
                throw new InvalidDateTimeException("Data de nascimento inválida: A data de nascimento não pode ser no futuro.");
            }
            if ((DateTime.Now.Year - updateDoctorDTO.BirthDate.ToDateTime().Year) < 18 || (DateTime.Now.Year - updateDoctorDTO.BirthDate.ToDateTime().Year) == 18 && (((DateTime.Now.Month == updateDoctorDTO.BirthDate.ToDateTime().Month) && (DateTime.Now.Day < updateDoctorDTO.BirthDate.ToDateTime().Day)) || DateTime.Now.Month < updateDoctorDTO.BirthDate.ToDateTime().Month))
            {
                throw new InvalidDateTimeException("Data de nascimento inválida: A idade não pode ser menor que 18 anos");
            }
            if (await _doctorRepository.ExistsByRMCAsync(updateDoctorDTO.RMC))
            {
                throw new InvalidRMCException("CRM já cadastrado.");
            }
            if ((await _doctorRepository.ExistsByCPFAsync(updateDoctorDTO.CPF)) || (await _patientRepository.ExistsByCPFAsync(updateDoctorDTO.CPF)))
            {
                throw new InvalidCPFException("CPF já cadastrado.");
            }
            if (await _doctorRepository.ExistsByEmailAsync(updateDoctorDTO.Email) || await _patientRepository.ExistsByEmailAsync(updateDoctorDTO.Email))
            {
                throw new InvalidEmailException("Email já cadastrado.");
            }

            await _doctorRepository.UpdateAsync(id, updateDoctorDTO.Name,
                updateDoctorDTO.LastName,
                updateDoctorDTO.Address,
                updateDoctorDTO.BirthDate.ToDateTime(),
                updateDoctorDTO.Gender,
                updateDoctorDTO.CPF.ToCPF(),
                updateDoctorDTO.Email.ToEmail(),
                updateDoctorDTO.Phone,
                updateDoctorDTO.RMC.ToRMC(),
                updateDoctorDTO.Speciality);

            var schedule = await _doctorRepository.GetDateTimeWork(id);

            foreach (var dateTimeWorkDTO in schedule)
            {
                var dateTimeWork = MapToDoctorDateTimeWorkResponseDTO(dateTimeWorkDTO);
                doctor.DateTimeWorkList.Add(dateTimeWork);
            }


            return new DoctorResponseDTO
            {
                Id = doctor.Id,
                Status = doctor.Status.ToString(),
                Name = updateDoctorDTO.Name,
                LastName = updateDoctorDTO.LastName,
                Email = updateDoctorDTO.Email,
                Phone = updateDoctorDTO.Phone,
                Address = updateDoctorDTO.Address,
                BirthDate = updateDoctorDTO.BirthDate,
                Gender = updateDoctorDTO.Gender.ToString(),
                Speciality = updateDoctorDTO.Speciality.ToString(),
                RMC = updateDoctorDTO.RMC,
                DateTimeWorkList = doctor.DateTimeWorkList
            };
        }
        public async Task<IEnumerable<DoctorScheduleResponseDTO>> UpdateDoctorScheduleAsync(int id, IEnumerable<UpdateDoctorScheduleDTO> updateDoctorScheduleDTO)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new ArgumentException("Médico não encontrado.");
            }

            foreach (var dateTimeWorkDTO in updateDoctorScheduleDTO)
            {
                if (dateTimeWorkDTO.DayOfWeek <= 0 || dateTimeWorkDTO.DayOfWeek > 7)
                {
                    throw new InvalidDateTimeException("Dia da semana inválido.");
                }
            }

            var dateTimeWorkList = updateDoctorScheduleDTO.Select(dto => new DateTimeWork
            {
                IdDoctor = doctor.Id,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime.ToTimeSpan(),
                EndTime = dto.EndTime.ToTimeSpan()
            }).ToList();
            await _doctorRepository.UpdateDateTimeWorkAsync(id, dateTimeWorkList);

            var dateTimeWorkListDto = updateDoctorScheduleDTO.Select(dto => new DoctorScheduleResponseDTO
            {
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            }).ToList();

            return dateTimeWorkListDto;
        }
        public async Task<DoctorResponseDTO> UpdateStatusAsync(int id, UpdateStatusDTO updateStatusDTO)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new KeyNotFoundException("Médico não encontrado.");
            }

            await _doctorRepository.UpdateStatusAsync(id, updateStatusDTO.Status);

            return new DoctorResponseDTO
            {
                Id = doctor.Id,
                Status = updateStatusDTO.Status.ToString(),
                Name = doctor.Name,
                LastName = doctor.LastName,
                Email = doctor.Email.ToString(),
                Phone = doctor.Phone,
                Address = doctor.Address,
                BirthDate = doctor.BirthDate.ToString("dd/MM/yyyy"),
                Gender = doctor.Gender.ToString(),
                Speciality = doctor.Speciality.ToString(),
                RMC = doctor.RMC.Value,
                DateTimeWorkList = doctor.DateTimeWorkList
            };
        }
    }
}
