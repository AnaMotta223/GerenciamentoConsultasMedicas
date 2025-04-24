using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Domain.ValueObjects;
using AppointmentsManager.Infrastructure.Configuration;
using Dapper;

namespace AppointmentsManager.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IDatabaseConfig _databaseConfig;

        public DoctorRepository(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            using var connection = _databaseConfig.GetConnection();
            const string query = "SELECT * FROM doctor";
            return await connection.QueryAsync<Doctor>(query);
        }
        public async Task<Doctor> GetByIdAsync(int id)
        {
            using var connection = _databaseConfig.GetConnection();
            const string query = "SELECT * FROM doctor WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Doctor>(query, new { Id = id });
        }
        public async Task<IEnumerable<DateTimeWork>> GetDateTimeWork(int id)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "SELECT * FROM work_schedule WHERE id_doctor = @Id";
            return await connection.QueryAsync<DateTimeWork>(query, new { Id = id });
        }
        public async Task<bool> ExistsByCPFAsync(string cpf)
        {
            using var connection = _databaseConfig.GetConnection();
            const string query = "SELECT 1 FROM doctor WHERE cpf = @CPF";
            return await connection.ExecuteScalarAsync<bool>(query, new { CPF = cpf });
        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            using var connection = _databaseConfig.GetConnection();
            const string query = "SELECT 1 FROM doctor WHERE email = @Email";
            return await connection.ExecuteScalarAsync<bool>(query, new { Email = email });
        }
        public async Task<bool> ExistsByRMCAsync(string rmc)
        {
            using var connection = _databaseConfig.GetConnection();
            const string query = "SELECT 1 FROM doctor WHERE rmc = @RMC";
            return await connection.ExecuteScalarAsync<bool>(query, new { RMC = rmc });
        }
        public async Task<Doctor> AddAsync(Doctor doctor)
        {
            const string query = @"
            INSERT INTO doctor (status, name, last_name, email, password, phone, address, birth_date, gender, cpf, rmc, speciality)
            VALUES (@Status, @Name, @LastName, @Email, @Password, @Phone, @Address, @BirthDate, @Gender, @CPF, @RMC, @Speciality)
            RETURNING id;"; 

            using var connection = _databaseConfig.GetConnection();

            var id = await connection.QuerySingleAsync<int>(query, new
            {
                doctor.Status,
                doctor.Name,
                doctor.LastName,
                doctor.Email,
                doctor.Password,
                doctor.Phone,
                doctor.Address,
                doctor.BirthDate,
                doctor.Gender,
                doctor.CPF,
                doctor.RMC,
                doctor.Speciality
            });

            doctor.Id = id;
            return doctor;
        }
        public async Task UpdateAsync(int id, string newName, string newLastName, string newAddress, DateTime newBirthDate, Gender newGender, CPF newCPF, Email newEmail, string newPhone, RMC rmc, Speciality newSpeciality)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "UPDATE doctor SET name = @Name, last_name = @LastName, address = @Address, birth_date = @BirthDate, gender = @Gender, cpf = @CPF, email = @Email, phone = @Phone, rmc = @RMC, speciality = @Speciality WHERE id = @Id";

            await connection.ExecuteAsync(query, new
            {
                Name = newName,
                LastName = newLastName,
                Address = newAddress,
                BirthDate = newBirthDate,
                Gender = (int)newGender,
                CPF = newCPF,
                Email = newEmail,
                Phone = newPhone,
                RMC = rmc,
                Speciality = (int)newSpeciality,
                Id = id
            });
        }
        public async Task<IEnumerable<DateTimeWork>> UpdateDateTimeWorkAsync(int id, IEnumerable<DateTimeWork> dateTimeWorkList)
        {
                using var connection = _databaseConfig.GetConnection();
                var deleteQuery = "DELETE FROM work_schedule WHERE id_doctor = @Id";
                await connection.ExecuteAsync(deleteQuery, new { Id = id });

                var insertQuery = "INSERT INTO work_schedule (id_doctor, day_of_week, start_time, end_time) VALUES (@IdDoctor, @DayOfWeek, @StartTime, @EndTime)";

                foreach (var dateTimeWork in dateTimeWorkList)
                {
                    await connection.ExecuteAsync(insertQuery, new
                    {
                        IdDoctor = id, 
                        DayOfWeek = dateTimeWork.DayOfWeek, 
                        StartTime = dateTimeWork.StartTime, 
                        EndTime = dateTimeWork.EndTime
                    });
                }
            return dateTimeWorkList;
        }
        public async Task UpdateStatusAsync(int id, UserStatus status)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "UPDATE doctor SET status = @Status WHERE id = @Id";
            await connection.ExecuteAsync(query, new { Id = id, Status = (int)status });
        }
        public async Task DeleteAsync(int id)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "DELETE FROM doctor WHERE id = @Id";
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
