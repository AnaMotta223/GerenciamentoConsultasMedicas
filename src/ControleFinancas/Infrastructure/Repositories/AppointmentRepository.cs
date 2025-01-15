using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Infrastructure.Configuration;
using Dapper;
using System.Numerics;

namespace AppointmentsManager.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IDatabaseConfig _databaseConfig;

        public AppointmentRepository(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "SELECT * FROM appointment WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Appointment>(query, new { Id = id });
        }
        public async Task<IEnumerable<Appointment>> GetByDateAsync(DateTime dateTime)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "SELECT * FROM appointment WHERE date_time::date = @DateTime";
            return await connection.QueryAsync<Appointment>(query, new { DateTime = dateTime.Date });
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "SELECT * FROM appointment";
            return await connection.QueryAsync<Appointment>(query);
        }
        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "INSERT INTO appointment (id_doctor, id_patient, date_time, status, notes) " +
                        "VALUES (@DoctorId, @PatientId, @DateTimeAppointment, @AppointmentStatus, @Notes) RETURNING id;";

            var id = await connection.QuerySingleAsync<int>(query, new
            {
                DoctorId = appointment.DoctorId,
                PatientId = appointment.PatientId,
                DateTimeAppointment = appointment.DateTimeAppointment,
                AppointmentStatus = (int)appointment.AppointmentStatus,
                Notes = appointment.Notes
            });
            appointment.Id = id;
            return appointment;
        }
        public async Task UpdateAsync(int id, DateTime dateTimeAppointment, AppointmentStatus appointmentStatus, string? notes)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "UPDATE appointment SET date_time = @DateTimeAppointment, status = @AppointmentStatus, notes = @Notes WHERE id = @Id";
            await connection.ExecuteAsync(query, new { DateTimeAppointment = dateTimeAppointment, AppointmentStatus = (int)appointmentStatus, Notes = notes, Id = id });
        }
        public async Task UpdateStatusAsync(int id, AppointmentStatus appointmentStatus, string? notes)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "UPDATE appointment SET status = @AppointmentStatus, notes = @Notes WHERE id = @Id";
            await connection.ExecuteAsync(query, new { AppointmentStatus = (int)appointmentStatus, Notes = notes, Id = id });
        }
        public async Task DeleteAsync(int id)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "DELETE FROM appointment WHERE id = @Id";
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
