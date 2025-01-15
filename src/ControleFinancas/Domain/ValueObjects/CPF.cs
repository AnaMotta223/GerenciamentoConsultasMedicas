using AppointmentsManager.Domain.Exceptions;

namespace AppointmentsManager.Domain.ValueObjects
{
    public class CPF
    {
        public string Value { get; }

        public CPF(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidCPFException("O CPF não pode ser vazio.");

            value = value.Replace(".", "").Replace("-", ""); 
            if (value.Length != 11 || !value.All(char.IsDigit) || !IsValid(value))
                throw new InvalidCPFException("O CPF informado é inválido.");

            Value = value;
        }
        public override string ToString() => Value;
        private bool IsValid(string cpf)
        {
            int[] multipliers1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multipliers2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string hasCpf = cpf.Substring(0, 9);
            int sum = multipliers1.Select((t, i) => t * int.Parse(hasCpf[i].ToString())).Sum();
            int mod = sum % 11;
            string digit = (mod < 2 ? 0 : 11 - mod).ToString();

            hasCpf += digit;
            sum = multipliers2.Select((t, i) => t * int.Parse(hasCpf[i].ToString())).Sum();
            mod = sum % 11;
            digit += (mod < 2 ? 0 : 11 - mod).ToString();

            return cpf.EndsWith(digit);
        }
        public override bool Equals(object obj)
            => obj is CPF other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }

}
