using AppointmentsManager.Domain.Exceptions;

namespace AppointmentsManager.Domain.ValueObjects
{
    public class RMC
    {
        public string Value { get; }

        public RMC(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidRMCException("O número do CRM não pode ser vazio.");

            if (value.Length > 20)
                throw new InvalidRMCException("O CRM deve ter no máximo 20 caracteres.");

            Value = value;
        }
        public override bool Equals(object obj)
            => obj is RMC other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }

}
