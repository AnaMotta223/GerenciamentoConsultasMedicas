using AppointmentsManager.Application.DTOs;
using AppointmentsManager.Application.Services;
using AppointmentsManager.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsManager.Presentation.Controllers
{
    [Route("api/schedules")]
    [ApiController]
    public class DateTimeWorkController : ControllerBase
    {
        private readonly DateTimeWorkService _dateTimeWorkService;

        public DateTimeWorkController(DateTimeWorkService dateTimeWorkService)
        {
            _dateTimeWorkService = dateTimeWorkService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchedule(int id)
        {
            try
            {
                var schedule = await _dateTimeWorkService.SearchScheduleAsync(id);
                return Ok(schedule);
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
        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateDateTimeWorkModel createDateTimeWorkModel)
        {
            try
            {
                var dateTimeWorkDto = createDateTimeWorkModel.ToDto();
                var dateTimeWork = await _dateTimeWorkService.CreateScheduleAsync(dateTimeWorkDto);
                return CreatedAtAction(nameof(GetSchedule), new { id = dateTimeWork.Id }, dateTimeWork);
            }
            catch (InvalidDateTimeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }
    }
}
