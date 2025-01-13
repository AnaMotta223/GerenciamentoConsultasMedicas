using Dapper;
using System.Data;

namespace AppointmentsManager.Domain.ValueObjects
{
    public class CPFTypeHandler : SqlMapper.TypeHandler<CPF>
    {
        public override CPF? Parse(object value)
        {
            return new CPF(value.ToString() ?? throw new ArgumentNullException(nameof(value)));
        }

        public override void SetValue(IDbDataParameter parameter, CPF value)
        {
            parameter.Value = value.Value;
        }
    }
}
