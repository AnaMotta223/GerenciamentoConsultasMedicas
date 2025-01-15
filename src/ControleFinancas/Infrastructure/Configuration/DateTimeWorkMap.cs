using AppointmentsManager.Domain.Entities;
using Dapper.FluentMap.Mapping;

namespace AppointmentsManager.Infrastructure.Configuration
{
    public class DateTimeWorkMap : EntityMap<DateTimeWork>
    {
        public DateTimeWorkMap()
        {
            Map(d => d.Id).ToColumn("id");
            Map(d => d.DayOfWeek).ToColumn("day_of_week");
            Map(d => d.StartTime).ToColumn("start_time");
            Map(d => d.EndTime).ToColumn("end_time");
            Map(d => d.IdDoctor).ToColumn("id_doctor");
        }
    }

}
