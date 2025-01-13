using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Interfaces;
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

        Task IPatientRepository.AddAsync(Patient patient)
        {
            throw new NotImplementedException();
        }

        Task IPatientRepository.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Patient>> IPatientRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Patient> IPatientRepository.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task IPatientRepository.UpdateAsync(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}
