using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Domain.Enums;

namespace AppointmentsManager.Presentation.Models
{
    public class UpdateStatusModel
    {
        public UserStatus Status { get; set; }
        public UpdateStatusDTO ToDto()
        {
            return new UpdateStatusDTO
            {
                Status = this.Status,
            };
        }
    }
}
