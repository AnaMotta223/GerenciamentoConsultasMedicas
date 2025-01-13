using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Domain.ValueObjects;
using Dapper;
using System.Data;

namespace AppointmentsManager.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IDbConnection _dbConnection;

        public PatientRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            var query = "SELECT * FROM patient";
            return await _dbConnection.QueryAsync<Patient>(query);
        }
        public async Task<Patient> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM patient WHERE id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Patient>(query, new { Id = id });
        }
        public async Task AddAsync(Patient patient)
        {
            var query = "INSERT INTO patient (name, last_name, email, password, phone, address, birth_date, gender, cpf) " +
                       "VALUES (@Name, @LastName, @Email, @Password, @Phone, @Address, @BirthDate, @Gender, @CPF) RETURNING id;";

            var parameters = new
            {
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
            var id = await _dbConnection.QuerySingleAsync<int>(query, parameters);
        }
        public async Task UpdateAsync(int id, string newName, string newLastName, string newPassword, string newAddress, DateTime newBirthDate, Gender newGender, Email newEmail, string newPhone)
        {
            var query = "UPDATE patient SET name = @Name, last_name = @LastName, password = @Password, address = @Address, birth_date = @BirthDate, gender = @Gender, email = @Email, phone = @Phone WHERE id = @Id";

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
                Id = id
            });
        }
        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM patient WHERE id = @Id";
            await _dbConnection.ExecuteAsync(query, new { Id = id });
        }
    }
}
