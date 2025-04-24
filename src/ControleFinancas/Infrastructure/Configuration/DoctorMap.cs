namespace AppointmentsManager.Infrastructure.Configuration
{
    using AppointmentsManager.Domain.Entities;
    using Dapper.FluentMap.Mapping;

    public class DoctorMap : EntityMap<Doctor>
    {
        public DoctorMap()
        {
            Map(d => d.Id).ToColumn("id");
            Map(d => d.Status).ToColumn("status");
            Map(d => d.Name).ToColumn("name");
            Map(d => d.LastName).ToColumn("last_name");
            Map(d => d.Email).ToColumn("email");
            Map(d => d.Password).ToColumn("password");
            Map(d => d.Phone).ToColumn("phone");
            Map(d => d.Address).ToColumn("address");
            Map(d => d.BirthDate).ToColumn("birth_date");
            Map(d => d.Gender).ToColumn("gender");
            Map(d => d.CPF).ToColumn("cpf");
            Map(d => d.RMC).ToColumn("rmc");
            Map(d => d.Speciality).ToColumn("speciality");
        }
    }

}
