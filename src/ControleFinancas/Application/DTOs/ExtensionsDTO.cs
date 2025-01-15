using AppointmentsManager.Domain.ValueObjects;

namespace AppointmentsManager.Application.DTOs
{
    public static class ExtensionsDTO
    {
        public static Email ToEmail(this string email)
        {
            return new Email(email);
        }
        public static CPF ToCPF(this string cpf)
        {
            return new CPF(cpf);
        }
        public static RMC ToRMC(this string rmc)
        {
            return new RMC(rmc);
        }
        public static DateTime ToDateTime(this string date)
        {
            return DateTime.Parse(date); 
        }
        public static TimeSpan ToTimeSpan(this string time)
        {
            return TimeSpan.Parse(time);
        }
    }
}
