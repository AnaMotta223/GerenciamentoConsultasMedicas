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

            if (value.Length != 9)
                throw new InvalidRMCException("CRM inválido. Deve conter exatamente 9 dígitos.");

            Value = value;
        }

        public override bool Equals(object obj)
            => obj is RMC other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();
    }
}
