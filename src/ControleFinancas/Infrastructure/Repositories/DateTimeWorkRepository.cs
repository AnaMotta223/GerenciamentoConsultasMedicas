using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Interfaces;
using AppointmentsManager.Infrastructure.Configuration;
using Dapper;

namespace AppointmentsManager.Infrastructure.Repositories
{
    public class DateTimeWorkRepository : IDateTimeWorkRepository
    {
        private readonly IDatabaseConfig _databaseConfig;
        public DateTimeWorkRepository(IDatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        public async Task<IEnumerable<DateTimeWork>> GetAllAsync()
        {
            using var connection = _databaseConfig.GetConnection();
            const string query = "SELECT * FROM work_schedule";
            return await connection.QueryAsync<DateTimeWork>(query);
        }
        public async Task<DateTimeWork> GetByIdAsync(int id)
        {
            using var connection = _databaseConfig.GetConnection();
            const string query = "SELECT * FROM work_schedule WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<DateTimeWork>(query, new { Id = id });
        }
        public async Task<DateTimeWork> AddAsync(DateTimeWork dateTimeWork)
        {
            using var connection = _databaseConfig.GetConnection();
            var query = "INSERT INTO work_schedule (day_of_week, start_time, end_time, id_doctor) " +
                        "VALUES (@DayOfWeek, @StartTime, @EndTime, @IdDoctor) RETURNING id;";

            var id = await connection.QuerySingleAsync<int>(query, new
            {
                dateTimeWork.DayOfWeek,
                dateTimeWork.StartTime,
                dateTimeWork.EndTime,
                dateTimeWork.IdDoctor
            });

            dateTimeWork.Id = id;
            return dateTimeWork;
        }
    }
}
