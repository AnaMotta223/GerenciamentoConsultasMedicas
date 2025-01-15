using AppointmentsManager.Application.Services;
using AppointmentsManager.Domain.Exceptions;
using AppointmentsManager.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsManager.Presentation.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            try
            {
                var appointments = await _appointmentService.ListAppointmentsAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            try
            {
                var appointment = await _appointmentService.SearchAppointmentAsync(id);
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }
        [HttpGet("date")]
        public async Task<IActionResult> SearchAppointmentsByDateAsync(DateTime dateTime)
        {
            try
            {
                var appointments = await _appointmentService.SearchAppointmentsByDateAsync(dateTime);
                return Ok(appointments);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }
        [HttpGet("availability/{id}")]
        public async Task<IActionResult> IsDoctorAvailableAsync(int id, DateTime appointmentDateTime)
        {
            try
            {
                bool isAvailable = await _appointmentService.IsDoctorAvailableAsync(id, appointmentDateTime);
                return Ok(isAvailable);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentModel createAppointmentModel)
        {
            try
            {
                var appointmentDto = createAppointmentModel.ToDto();
                var appointment = await _appointmentService.ScheduleAppointmentAsync(appointmentDto);
                return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
            }
            catch (InvalidDateTimeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentModel updateAppointmentModel)
        {
            try
            {
                var appointmentDto = updateAppointmentModel.ToDto();
                var appointment = await _appointmentService.UpdateAppointmentAsync(id, appointmentDto);
                return Ok(appointment);
            }
            catch (InvalidDateTimeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, [FromBody] UpdateStatusAppointmentModel updateStatusAppointmentModel)
        {
            try
            {
                var appointmentDto = updateStatusAppointmentModel.ToDto();
                var appointment = await _appointmentService.UpdateAppointmentStatusAsync(id, appointmentDto);
                return Ok(appointment);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                await _appointmentService.DeleteAppointmentAsync(id);
                return NoContent(); 
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }
    }
}
