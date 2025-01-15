using AppointmentsManager.Domain.Entities;

namespace AppointmentsManager.Domain.Interfaces
{
    public interface IDateTimeWorkRepository
    {
        Task<IEnumerable<DateTimeWork>> GetAllAsync();
        Task<DateTimeWork> GetByIdAsync(int id);
        Task<DateTimeWork> AddAsync(DateTimeWork dateTimeWork);
    }
}
