﻿using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.ValueObjects;

namespace AppointmentsManager.Domain.Interfaces
{
    public interface IDoctorRepository
    {
        Task<bool> ExistsByCPFAsync(string cpf);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByRMCAsync(string rmc);
        Task<Doctor> GetByIdAsync(int id);
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<IEnumerable<DateTimeWork>> GetDateTimeWork(int id);
        Task<IEnumerable<Doctor>> GetByStatusAsync(UserStatus status);
        Task<Doctor> AddAsync(Doctor doctor);
        Task UpdateAsync(int id, string newName, string newLastName, string newAddress, DateTime newBirthDate, Gender newGender, CPF newCPF, Email newEmail, string newPhone, RMC rmc, Speciality newSpeciality);
        Task<IEnumerable<DateTimeWork>> UpdateDateTimeWorkAsync(int id, IEnumerable<DateTimeWork> dateTimeWorkList);
        Task UpdateStatusAsync(int id, UserStatus status);
        Task DeleteAsync(int id);
    }
}
