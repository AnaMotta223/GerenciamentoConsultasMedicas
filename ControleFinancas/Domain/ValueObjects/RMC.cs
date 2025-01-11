namespace AppointmentsManager.Domain.ValueObjects
{
    public class RMC
    {
        public string Value { get; }

        public RMC(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("O número do CRM não pode ser vazio.");

            if (value.Length > 20)
                throw new ArgumentException("O CRM deve ter no máximo 20 caracteres.");

            Value = value;
        }
        public override bool Equals(object obj)
            => obj is RMC other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }

}
