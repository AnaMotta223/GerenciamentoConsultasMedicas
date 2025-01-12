using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Interfaces;
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

        Task IDoctorRepository.AddAsync(Doctor doctor)
        {
            throw new NotImplementedException();
        }

        Task IDoctorRepository.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Doctor>> IDoctorRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Doctor> IDoctorRepository.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task IDoctorRepository.UpdateAsync(Doctor doctor)
        {
            throw new NotImplementedException();
        }

        Task IDoctorRepository.UpdateDateTimeWorkAsync(Doctor doctor)
        {
            throw new NotImplementedException();
        }
    }
}
