namespace AppointmentsManager.Domain.Exceptions
{
    public class InvalidCPFException : ArgumentException
    {
        public InvalidCPFException(string message) : base(message) { }
    }
}
