using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Domain.ValueObjects;
using Dapper;
using System.Data;

namespace AppointmentsManager.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IDbConnection _dbConnection;

        public DoctorRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            var query = "SELECT * FROM doctor";
            return await _dbConnection.QueryAsync<Doctor>(query);
        }
        public async Task<Doctor> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM doctor WHERE id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Doctor>(query, new { Id = id });
        }
        public async Task<IEnumerable<DateTimeWork>> GetDateTimeWork(int id)
        {
            var query = "SELECT * FROM work_schedule WHERE id_doctor = @Id";
            return await _dbConnection.QueryAsync<DateTimeWork>(query, new { Id = id });
        }
        public async Task AddAsync(Doctor doctor)
        {
            var query = "INSERT INTO doctor (name, last_name, email, password, phone, address, birth_date, gender, cpf, rmc, speciality) " +
                       "VALUES (@Name, @LastName, @Email, @Password, @Phone, @Address, @BirthDate, @Gender, @CPF, @RMC, @Speciality) RETURNING id;";

            var parameters = new
            {
                Name = doctor.Name,
                LastName = doctor.LastName,
                Email = doctor.Email,
                Password = doctor.Password,
                Phone = doctor.Phone,
                Address = doctor.Address,
                BirthDate = doctor.BirthDate,
                Gender = (int)doctor.Gender,
                CPF = doctor.CPF,
                RMC = doctor.RMC,
                Speciality = (int)doctor.Speciality
            };
            var id = await _dbConnection.QuerySingleAsync<int>(query, parameters);
        }
        public async Task UpdateAsync(int id, string newName, string newLastName, string newPassword, string newAddress, DateTime newBirthDate, Gender newGender, Email newEmail, string newPhone, RMC rmc, Speciality newSpeciality)
        {
            var query = "UPDATE doctor SET name = @Name, last_name = @LastName, password = @Password, address = @Address, birth_date = @BirthDate, gender = @Gender, email = @Email, phone = @Phone, rmc = @RMC, speciality = @Speciality WHERE id = @Id";

            await _dbConnection.ExecuteAsync(query, new
            {
                Name = newName,
                LastName = newLastName,
                Password = newPassword,
                Address = newAddress,
                BirthDate = newBirthDate,
                Gender = (int)newGender,
                Email = newEmail,
                Phone = newPhone,
                RMC = rmc,
                Speciality = (int)newSpeciality,
                Id = id
            });
        }
        public async Task UpdateDateTimeWorkAsync(int id, List<DateTimeWork> dateTimeWorkList)
        {
                var deleteQuery = "DELETE FROM work_schedule WHERE id_doctor = @Id";
                await _dbConnection.ExecuteAsync(deleteQuery, new { Id = id });

                var insertQuery = "INSERT INTO work_schedule (id_doctor, day_of_week, start_time, end_time) VALUES (@DoctorId, @DayOfWeek, @StartTime, @EndTime)";

                foreach (var dateTimeWork in dateTimeWorkList)
                {
                    await _dbConnection.ExecuteAsync(insertQuery, new
                    {
                        DoctorId = id, 
                        DayOfWeek = dateTimeWork.DayOfWeek, 
                        StartTime = dateTimeWork.StartTime, 
                        EndTime = dateTimeWork.EndTime
                    });
                }
        }
        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM doctor WHERE id = @Id";
            await _dbConnection.ExecuteAsync(query, new { Id = id });
        }
    }
}
