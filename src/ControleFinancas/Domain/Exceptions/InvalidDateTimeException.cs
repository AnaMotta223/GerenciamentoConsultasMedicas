namespace AppointmentsManager.Domain.Exceptions
{
    public class InvalidDateTimeException : Exception
    {
        public InvalidDateTimeException(string message) : base(message) { }
    }
}
