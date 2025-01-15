namespace AppointmentsManager.Domain.Exceptions
{
    public class InvalidPhoneException : ArgumentException
    {
        public InvalidPhoneException(string message) : base(message) { }
    }
}
