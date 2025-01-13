namespace AppointmentsManager.Domain.Exceptions
{
    public class ForbiddenException : UnauthorizedAccessException
    {
        public ForbiddenException(string message) : base(message) { }
    }

}
