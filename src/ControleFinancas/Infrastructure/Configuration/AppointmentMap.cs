using AppointmentsManager.Domain.Entities;
using Dapper.FluentMap.Mapping;

namespace AppointmentsManager.Infrastructure.Configuration
{
    public class AppointmentMap : EntityMap<Appointment>
    {
        public AppointmentMap()
        {
            Map(d => d.Id).ToColumn("id");
            Map(d => d.DoctorId).ToColumn("id_doctor");
            Map(d => d.PatientId).ToColumn("id_patient");
            Map(d => d.DateTimeAppointment).ToColumn("date_time");
            Map(d => d.AppointmentStatus).ToColumn("status");
            Map(d => d.Notes).ToColumn("notes");
        }
    }
}
