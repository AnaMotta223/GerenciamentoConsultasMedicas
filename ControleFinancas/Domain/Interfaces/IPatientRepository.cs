using AppointmentsManager.Domain.Entities;

namespace AppointmentsManager.Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> GetByIdAsync(int id);
        Task<IEnumerable<Patient>> GetAllAsync();
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(int id);
    }
}
