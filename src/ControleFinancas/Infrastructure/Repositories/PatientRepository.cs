using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Domain.ValueObjects;
using AppointmentsManager.Infrastructure.Configuration;
using Dapper;

namespace AppointmentsManager.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IDatabaseConfig _databaseConfig;

        public PatientRepository(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "SELECT * FROM patient";
            return await connection.QueryAsync<Patient>(query);
        }
        public async Task<Patient> GetByIdAsync(int id)
        {
            using var connection = _databaseConfig.GetConnection();
            const string query = "SELECT * FROM patient WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Patient>(query, new { Id = id });
        }
        public async Task<bool> ExistsByCPFAsync(string cpf)
        {
            using var connection = _databaseConfig.GetConnection();
            const string query = "SELECT 1 FROM patient WHERE cpf = @CPF";
            return await connection.ExecuteScalarAsync<bool>(query, new { CPF = cpf });
        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            using var connection = _databaseConfig.GetConnection();
            const string query = "SELECT 1 FROM patient WHERE email = @Email";
            return await connection.ExecuteScalarAsync<bool>(query, new { Email = email });
        }
        public async Task<IEnumerable<Patient>> GetByStatusAsync(UserStatus status)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "SELECT * FROM patient WHERE status = @Status";
            return await connection.QueryAsync<Patient>(query, new { Status = status });
        }
        public async Task AddAsync(Patient patient)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "INSERT INTO patient (status, name, last_name, email, password, phone, address, birth_date, gender, cpf) " +
                        "VALUES (@Status, @Name, @LastName, @Email, @Password, @Phone, @Address, @BirthDate, @Gender, @CPF) RETURNING id;";

            var parameters = new
            {
                Status = patient.Status,
                Name = patient.Name,
                LastName = patient.LastName,
                Email = patient.Email,
                Password = patient.Password,
                Phone = patient.Phone,
                Address = patient.Address,
                BirthDate = patient.BirthDate,
                Gender = (int)patient.Gender,
                CPF = patient.CPF,
            };
            patient.Id = await connection.ExecuteScalarAsync<int>(query, parameters);
        }
        public async Task UpdateAsync(int id, string newName, string newLastName, string newAddress, DateTime newBirthDate, Gender newGender, CPF newCPF, Email newEmail, string newPhone)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "UPDATE patient SET name = @Name, last_name = @LastName, address = @Address, birth_date = @BirthDate, gender = @Gender, cpf = @CPF, email = @Email, phone = @Phone WHERE id = @Id";

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
                Id = id
            });
        }
        public async Task UpdateStatusAsync(int id, UserStatus status)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "UPDATE patient SET status = @Status WHERE id = @Id";
            await connection.ExecuteAsync(query, new { Id = id, Status = (int)status });
        }
    }
}
