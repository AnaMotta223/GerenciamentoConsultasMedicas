using Dapper;
using System.Data;

namespace AppointmentsManager.Domain.ValueObjects
{
    public class RMCTypeHandler : SqlMapper.TypeHandler<RMC>
    {
        public override RMC? Parse(object value)
        {
            return new RMC(value.ToString() ?? throw new ArgumentNullException(nameof(value)));
        }

        public override void SetValue(IDbDataParameter parameter, RMC value)
        {
            parameter.Value = value.Value;
        }
    }
}
