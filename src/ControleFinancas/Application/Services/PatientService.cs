using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Exceptions;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Utils;

namespace AppointmentsManager.Application.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly PasswordEncrypter _passwordEncrypter;

        public PatientService(IPatientRepository patientRepository, PasswordEncrypter passwordEncrypter)
        {
            _patientRepository = patientRepository;
            _passwordEncrypter = passwordEncrypter;
        }
        public async Task<IEnumerable<PatientResponseDTO>> ListPatientsAsync()
        {
            try
            {
                var patients = await _patientRepository.GetAllAsync();

                return patients?.Select(patient => new PatientResponseDTO
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    LastName = patient.LastName,
                    Email = patient.Email,
                    Phone = patient.Phone,
                    Address = patient.Address,
                    BirthDate = patient.BirthDate.ToString("dd/MM/yyyy"),
                    Gender = patient.Gender
                }) ?? new List<PatientResponseDTO>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ocorreu um erro ao listar os pacientes.", ex);
            }
        }
        public async Task<PatientResponseDTO> SearchPatientAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                throw new KeyNotFoundException($"Nenhum paciente encontrado com o ID {id}.");
            }

            return new PatientResponseDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                LastName = patient.LastName,
                Email = patient.Email,
                Phone = patient.Phone,
                Address = patient.Address,
                BirthDate = patient.BirthDate.ToString("dd/MM/yyyy"),
                Gender = patient.Gender
            };
        }
        public async Task<PatientResponseDTO> RegisterPatientAsync(CreatePatientDTO createPatientDTO)
        {
            var patients = await _patientRepository.GetAllAsync();

            if (createPatientDTO.BirthDate >= DateTime.Now)
            {
                throw new InvalidDateTimeException("Data de nascimento inválida.");
            }
            if (patients.Any(a => a.CPF.Equals(createPatientDTO.CPF)))
            {
                throw new InvalidCPFException("CPF já cadastrado.");
            }
            if (patients.Any(a => a.Email.Equals(createPatientDTO.Email)))
            {
                throw new InvalidEmailException("Email já cadastrado.");
            }
            var patient = new Patient
            {
                Name = createPatientDTO.Name,
                LastName = createPatientDTO.LastName,
                Email = createPatientDTO.Email,
                Password = _passwordEncrypter.HashPassword(createPatientDTO.Password),
                Phone = createPatientDTO.Phone,
                Address = createPatientDTO.Address,
                BirthDate = createPatientDTO.BirthDate,
                Gender = createPatientDTO.Gender,
                CPF = createPatientDTO.CPF
            };

            await _patientRepository.AddAsync(patient);

            var patientDto = new PatientResponseDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                LastName = patient.LastName,
                Email = patient.Email,
                Phone = patient.Phone,
                Address = patient.Address,
                BirthDate = patient.BirthDate.ToString("dd/MM/yyyy"),
                Gender = patient.Gender
            };

            return patientDto;
        }
        public async Task<PatientResponseDTO> UpdatePatientAsync(int id, UpdatePatientDTO updatePatientDTO)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            var patients = await _patientRepository.GetAllAsync();

            if (patient == null)
            {
                throw new ArgumentException("Paciente não encontrado.");
            }
            if (updatePatientDTO.BirthDate >= DateTime.Now)
            {
                throw new InvalidDateTimeException("Data de nascimento inválida.");
            }
            if (patients.Any(a => a.Email.Equals(updatePatientDTO.Email)))
            {
                throw new InvalidEmailException("Email já cadastrado.");
            }
            await _patientRepository.UpdateAsync(id, updatePatientDTO.Name,
                updatePatientDTO.LastName,
                _passwordEncrypter.HashPassword(updatePatientDTO.Password),
                updatePatientDTO.Address,
                updatePatientDTO.BirthDate,
                updatePatientDTO.Gender,
                updatePatientDTO.Email,
                updatePatientDTO.Phone);
            return new PatientResponseDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                LastName = patient.LastName,
                Email = patient.Email,
                Phone = patient.Phone,
                Address = patient.Address,
                BirthDate = patient.BirthDate.ToString("dd/MM/yyyy"),
                Gender = patient.Gender
            };
        }
        public async Task DeletePatientAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new ArgumentException("Paciente não encontrado.");
            }
            await _patientRepository.DeleteAsync(id);
        }
    }
}
