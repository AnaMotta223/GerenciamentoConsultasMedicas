using Npgsql;
using System.Data;

namespace AppointmentsManager.Infrastructure.Configuration
{
    public class DatabaseConfig : IDatabaseConfig
    {
        private readonly IConfiguration _configuration;

        public DatabaseConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            return new NpgsqlConnection(connectionString);
        }
    }
}
