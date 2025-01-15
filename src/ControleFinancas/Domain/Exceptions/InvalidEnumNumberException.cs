namespace AppointmentsManager.Domain.Exceptions
{
    public class InvalidEnumNumberException : ArgumentOutOfRangeException
    {
        public InvalidEnumNumberException(string message) : base(message) { }
    }
}
