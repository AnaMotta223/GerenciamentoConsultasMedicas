using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetByIdAsync(int id); 
        Task<IEnumerable<Appointment>> GetAllAsync(); 
        Task AddAsync(Appointment appointment); 
        Task UpdateAsync(int id, DateTime newDateTime, AppointmentStatus newStatus, string? notes = null);
        Task UpdateStatusAsync(int id, AppointmentStatus newStatus, string? notes = null);
        Task DeleteAsync(int id);
        Task<bool> HasConflictAsync(int doctorId, DateTime dateTimeAppointment);
    }
}
