namespace AppointmentsManager.Domain.Exceptions
{
    public class InvalidRMCException : ArgumentException
    {
        public InvalidRMCException(string message) : base(message) { }
    }
}
