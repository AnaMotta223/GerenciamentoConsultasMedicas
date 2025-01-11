using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Interfaces;
using Dapper;
using System.Data;

namespace AppointmentsManager.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly IDbConnection _connection;

        public AppointmentRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        //acabar de criar as queries

        public async Task<Appointment> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Appointment WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Appointment>(query, new { Id = id });
        }

        public async Task AddAsync(Appointment appointment)
        {
            var query = "INSERT INTO Appointment (DoctorId, PatientId, AppointmentDate) VALUES (@DoctorId, @PatientId, @AppointmentDate)";
            await _connection.ExecuteAsync(query, appointment);
        }

        public async Task<bool> HasConflict(Doctor doctor, DateTime dateTimeAppointment)
        {
            var query = @"
            SELECT COUNT(*) 
            FROM Appointments 
            WHERE DoctorId = @DoctorId AND DateTimeAppointment = @DateTimeAppointment";

            var count = await _connection.ExecuteScalarAsync<int>(query, new
            {
                DoctorId = doctor.Id,  
                DateTimeAppointment = dateTimeAppointment
            });

            return count > 0;
        }


        Task<IEnumerable<Appointment>> IAppointmentRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task IAppointmentRepository.UpdateDateTimeAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        Task IAppointmentRepository.UpdateStatusAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        Task IAppointmentRepository.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        //Task<Appointment> IAppointmentRepository.GetByIdAsync(int id)
        //{
        //    var query = "SELECT * FROM Appointments WHERE Id = @Id";
        //    return _connection.QueryFirstOrDefaultAsync<Appointment>(query, new { Id = id });
        //}

    }
}
