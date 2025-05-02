using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.ValueObjects;

namespace AppointmentsManager.Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<bool> ExistsByCPFAsync(string cpf);
        Task<bool> ExistsByEmailAsync(string email);
        Task<Patient> GetByIdAsync(int id);
        Task<IEnumerable<Patient>> GetByStatusAsync(UserStatus status);
        Task<IEnumerable<Patient>> GetAllAsync();
        Task AddAsync(Patient patient);
        Task UpdateAsync(int id, string newName, string newLastName, string newAddress, DateTime newBirthDate, Gender newGender, CPF newCPF, Email newEmail, string newPhone);
        Task UpdateStatusAsync(int id, UserStatus status);
    }
}
