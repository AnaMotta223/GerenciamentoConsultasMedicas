using System.Data;

namespace AppointmentsManager.Infrastructure.Configuration
{
    public interface IDatabaseConfig
    {
        IDbConnection GetConnection();
    }
}
