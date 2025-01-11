using AppointmentsManager.Domain.Entities;

namespace AppointmentsManager.Domain.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor> GetByIdAsync(int id);
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task AddAsync(Doctor doctor);
        Task UpdateAsync(Doctor doctor);
        //atualizar apenas os horários de trabalho
        Task UpdateDateTimeWorkAsync(Doctor doctor);
        Task DeleteAsync(int id);
    }
}
