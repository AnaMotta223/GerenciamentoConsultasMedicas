namespace AppointmentsManager.Domain.Exceptions
{
    public class InvalidAppointmentDateException : Exception
    {
        public InvalidAppointmentDateException(string message) : base(message) { }
    }
}
