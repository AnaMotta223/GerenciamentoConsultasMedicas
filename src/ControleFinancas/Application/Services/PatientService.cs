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
        public async Task<IEnumerable<PatientResponseDTO>> SearchByStatusAsync(UserStatus status)
        {
            var patients = await _patientRepository.GetByStatusAsync(status);
            if ((int)status < 0 || (int)status > 2)
            {
                throw new InvalidEnumNumberException("O status informado não existe.");
            }
            return patients.Select(patient => MapToPatientResponseDTO(patient));
        }
        public async Task<PatientResponseDTO> RegisterPatientAsync(CreatePatientDTO createPatientDTO)
        {
            if (createPatientDTO.Phone.Length < 8 || createPatientDTO.Phone.Length > 11)
            {
                throw new InvalidPhoneException("Número de telefone inválido. Deve ter no de 8 a 11 dígitos.");
            }
            if (((int)createPatientDTO.Gender) < 0 || ((int)createPatientDTO.Gender) > 3)
            {
                throw new InvalidEnumNumberException("Gênero inválido. Valor não encontrado.");
            }
            if (createPatientDTO.BirthDate.ToDateTime() >= DateTime.Now)
            {
                throw new InvalidDateTimeException("Data de nascimento inválida: A data de nascimento não pode ser no futuro.");
            }
            if (await _patientRepository.ExistsByCPFAsync(createPatientDTO.CPF) || await _doctorRepository.ExistsByCPFAsync(createPatientDTO.CPF))
            {
                throw new InvalidCPFException("CPF já cadastrado.");
            }
            if (await _patientRepository.ExistsByEmailAsync(createPatientDTO.Email) || await _doctorRepository.ExistsByEmailAsync(createPatientDTO.Email)) 
            {
                throw new InvalidEmailException("Email já cadastrado.");
            }
            
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
            if (updatePatientDTO.Phone.Length < 8 || updatePatientDTO.Phone.Length > 11)
            {
                throw new InvalidPhoneException("Número de telefone inválido. Deve ter de 8 a 11 dígitos.");
            }
            if (((int)updatePatientDTO.Gender) < 0 || ((int)updatePatientDTO.Gender) > 3)
            {
                throw new InvalidEnumNumberException("Gênero inválido. Valor não encontrado.");
            }
            if (updatePatientDTO.BirthDate.ToDateTime() >= DateTime.Now)
            {
                throw new InvalidDateTimeException("Data de nascimento inválida: A data de nascimento não pode ser no futuro.");
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
        public async Task<PatientResponseDTO> UpdateStatusAsync(int id, UpdateStatusDTO updateStatusDTO)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new KeyNotFoundException("Paciente não encontrado.");
            }
            if ((int)updateStatusDTO.Status < 0 || (int)updateStatusDTO.Status > 2)
            {
                throw new InvalidEnumNumberException("Status inválido. Valor não encontrado.");
            }

            await _patientRepository.UpdateStatusAsync(id, updateStatusDTO.Status);

            return new PatientResponseDTO
            {
                Id = patient.Id,
                Status = updateStatusDTO.Status.ToString(),
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
