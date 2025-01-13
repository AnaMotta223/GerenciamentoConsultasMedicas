using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Interfaces;
using Dapper;
using System.Data;

namespace AppointmentsManager.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IDbConnection _dbConnection;

        public AppointmentRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<Appointment> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM appointment WHERE id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Appointment>(query, new { Id = id });
        }
        public async Task<Appointment> GetByDateTimeAsync(DateTime dateTime)
        {
            var query = "SELECT * FROM appointment WHERE date_time = @DateTime";
            return await _dbConnection.QuerySingleOrDefaultAsync<Appointment>(query, new { DateTime = dateTime });
        }
        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            var query = "SELECT * FROM appointment";
            return await _dbConnection.QueryAsync<Appointment>(query);
        }
        public async Task AddAsync(Appointment appointment)
        {
            var query = "INSERT INTO appointment (id_doctor, id_patient, date_time, status, notes) " +
                        "VALUES (@DoctorId, @PatientId, @DateTimeAppointment, @AppointmentStatus, @Notes) RETURNING id;";

            var parameters = new
            {
                DoctorId = appointment.Doctor.Id,
                PatientId = appointment.Patient.Id,
                DateTimeAppointment = appointment.DateTimeAppointment,
                AppointmentStatus = (int)appointment.AppointmentStatus,
                Notes = appointment.Notes
            };
            var id = await _dbConnection.QuerySingleAsync<int>(query, parameters);
        }
        public async Task UpdateAsync(int id, DateTime dateTimeAppointment, AppointmentStatus appointmentStatus, string? notes = null)
        {
            var query = "UPDATE appointment SET date_time = @DateTimeAppointment, status = @AppointmentStatus, notes = @Notes WHERE id = @Id";
            await _dbConnection.ExecuteAsync(query, new { DateTimeAppointment = dateTimeAppointment, AppointmentStatus = (int)appointmentStatus, Notes = notes, Id = id });
        }
        public async Task UpdateStatusAsync(int id, AppointmentStatus appointmentStatus, string? notes = null)
        {
            var query = "UPDATE appointment SET status = @AppointmentStatus, notes = @Notes WHERE id = @Id";
            await _dbConnection.ExecuteAsync(query, new { AppointmentStatus = (int)appointmentStatus, Notes = notes, Id = id });
        }
        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM appointment WHERE id = @Id";
            await _dbConnection.ExecuteAsync(query, new { Id = id });
        }
    }
}
