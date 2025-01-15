using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetByIdAsync(int id); 
        Task<IEnumerable<Appointment>> GetAllAsync(); 
        Task<IEnumerable<Appointment>> GetByDateAsync(DateTime dateTime);
        Task<Appointment> AddAsync(Appointment appointment); 
        Task UpdateAsync(int id, DateTime newDateTime, AppointmentStatus newStatus, string? notes);
        Task UpdateStatusAsync(int id, AppointmentStatus newStatus, string? notes);
        Task DeleteAsync(int id);
    }
}
