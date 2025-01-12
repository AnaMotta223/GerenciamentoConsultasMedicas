namespace AppointmentsManager.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsCpfUniqueAsync(string cpf);
    }
}
