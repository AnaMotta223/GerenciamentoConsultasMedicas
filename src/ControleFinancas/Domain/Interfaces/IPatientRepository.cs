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
        Task<IEnumerable<Patient>> GetAllAsync();
        Task AddAsync(Patient patient);
        Task UpdateAsync(int id, string newName, string newLastName, string newPassword, string newAddress, DateTime newBirthDate, Gender newGender, Email newEmail, string newPhone);
        Task DeleteAsync(int id);
    }
}
