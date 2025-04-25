using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Exceptions;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Infrastructure.Repositories;
using AppointmentsManager.Utils;

namespace AppointmentsManager.Application.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly PasswordEncrypter _passwordEncrypter;

        public PatientService(IPatientRepository patientRepository, PasswordEncrypter passwordEncrypter, IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _passwordEncrypter = passwordEncrypter;
        }
        private static PatientResponseDTO MapToPatientResponseDTO(Patient patient)
        {
            return new PatientResponseDTO
            {
                Id = patient.Id,
                Status = patient.Status.ToString(),
                Name = patient.Name,
                LastName = patient.LastName,
                Email = patient.Email.ToString(),
                Phone = patient.Phone,
                Address = patient.Address,
                BirthDate = patient.BirthDate.ToString("dd/MM/yyyy"),
                Gender = patient.Gender.ToString()
            };
        }
        public async Task<IEnumerable<PatientResponseDTO>> ListPatientsAsync()
        {
            var patients = await _patientRepository.GetAllAsync();
            return patients.Select(patient => MapToPatientResponseDTO(patient));
        }
        public async Task<PatientResponseDTO> SearchPatientAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id)
                          ?? throw new KeyNotFoundException($"Nenhum paciente encontrado com o ID {id}.");
            return MapToPatientResponseDTO(patient);
        }
        public async Task<PatientResponseDTO> RegisterPatientAsync(CreatePatientDTO createPatientDTO)
        {
            if (createPatientDTO.Phone.Length < 8)
            {
                throw new InvalidPhoneException("Número de telefone inválido.");
            }
            if (((int)createPatientDTO.Gender) < 0 || ((int)createPatientDTO.Gender) > 3)
            {
                throw new InvalidEnumNumberException("Gênero inválido. Valor não encontrado.");
            }
            if (createPatientDTO.BirthDate.ToDateTime() >= DateTime.Now)
                throw new InvalidDateTimeException("Data de nascimento inválida.");

            if (await _patientRepository.ExistsByCPFAsync(createPatientDTO.CPF) || await _doctorRepository.ExistsByCPFAsync(createPatientDTO.CPF))
                throw new InvalidCPFException("CPF já cadastrado.");

            if (await _patientRepository.ExistsByEmailAsync(createPatientDTO.Email))
                throw new InvalidEmailException("Email já cadastrado.");
            
            var patient = new Patient
            {
                Status = UserStatus.ACTIVE,
                Name = createPatientDTO.Name,
                LastName = createPatientDTO.LastName,
                Email = createPatientDTO.Email.ToEmail(),
                Password = _passwordEncrypter.HashPassword(createPatientDTO.Password),
                Phone = createPatientDTO.Phone,
                Address = createPatientDTO.Address,
                BirthDate = createPatientDTO.BirthDate.ToDateTime(),
                Gender = createPatientDTO.Gender,
                CPF = createPatientDTO.CPF.ToCPF()
            };

            await _patientRepository.AddAsync(patient);
            return MapToPatientResponseDTO(patient);
        }
        public async Task<PatientResponseDTO> UpdatePatientAsync(int id, UpdatePatientDTO updatePatientDTO)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            var patients = await _patientRepository.GetAllAsync();

            if (patient == null)
            {
                throw new ArgumentException("Paciente não encontrado.");
            }
            if (updatePatientDTO.Phone.Length < 8)
            {
                throw new InvalidPhoneException("Número de telefone inválido.");
            }
            if (((int)updatePatientDTO.Gender) < 0 || ((int)updatePatientDTO.Gender) > 3)
            {
                throw new InvalidEnumNumberException("Gênero inválido. Valor não encontrado.");
            }
            if (updatePatientDTO.BirthDate.ToDateTime() >= DateTime.Now)
            {
                throw new InvalidDateTimeException("Data de nascimento inválida.");
            }
            if (patients.Any(p => p.Email.Equals(updatePatientDTO.Email)))
            {
                throw new InvalidEmailException("Email já cadastrado.");
            }
            await _patientRepository.UpdateAsync(id, updatePatientDTO.Name,
                updatePatientDTO.LastName,
                updatePatientDTO.Address,
                updatePatientDTO.BirthDate.ToDateTime(),
                updatePatientDTO.Gender,
                updatePatientDTO.CPF.ToCPF(),
                updatePatientDTO.Email.ToEmail(),
                updatePatientDTO.Phone);

            return new PatientResponseDTO
            {
                Id = patient.Id,
                Status = patient.Status.ToString(),
                Name = patient.Name,
                LastName = patient.LastName,
                Email = patient.Email.ToString(),
                Phone = patient.Phone,
                Address = patient.Address,
                BirthDate = patient.BirthDate.ToString("dd/MM/yyyy"),
                Gender = patient.Gender.ToString()
            };
        }
        public async Task<PatientResponseDTO> UpdateStatusAsync(int id, UserStatus status)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new KeyNotFoundException("Paciente não encontrado.");
            }

            await _patientRepository.UpdateStatusAsync(id, status);

            return new PatientResponseDTO
            {
                Id = patient.Id,
                Status = patient.Status.ToString(),
                Name = patient.Name,
                LastName = patient.LastName,
                Email = patient.Email.ToString(),
                Phone = patient.Phone,
                Address = patient.Address,
                BirthDate = patient.BirthDate.ToString("dd/MM/yyyy"),
                Gender = patient.Gender.ToString()
            };
        }

    }
}
