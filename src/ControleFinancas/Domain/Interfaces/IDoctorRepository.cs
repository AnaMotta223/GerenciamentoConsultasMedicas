using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.ValueObjects;

namespace AppointmentsManager.Domain.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor> GetByIdAsync(int id);
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<IEnumerable<DateTimeWork>> GetDateTimeWork(int id);
        Task AddAsync(Doctor doctor);
        Task UpdateAsync(int id, string newName, string newLastName, string newPassword, string newAddress, DateTime newBirthDate, Gender newGender, Email newEmail, string newPhone, RMC rmc, Speciality newSpeciality);
        Task UpdateDateTimeWorkAsync(int id, List<DateTimeWork> dateTimeWorkList);
        Task DeleteAsync(int id);
    }
}
