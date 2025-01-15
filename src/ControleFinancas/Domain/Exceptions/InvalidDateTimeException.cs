namespace AppointmentsManager.Domain.Exceptions
{
    public class InvalidDateTimeException : ArgumentException
    {
        public InvalidDateTimeException(string message) : base(message) { }
    }
}
