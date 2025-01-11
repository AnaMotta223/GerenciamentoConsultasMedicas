using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task AddAsync(Appointment appointment);
        //mudar a data e hora da consulta apenas 
        Task UpdateDateTimeAsync(int id, DateTime newDateTime);
        //mudar o status da consulta apenas
        Task UpdateStatusAsync(int id, AppointmentStatus newStatus, string? notes = null);
        Task DeleteAsync(int id);
        Task<bool> HasConflict(int id, DateTime dateTimeAppointment);
    }
}
