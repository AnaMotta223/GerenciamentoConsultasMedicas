using AppointmentsManager.Application.Services;
using AppointmentsManager.Domain.Entities;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Exceptions;
using AppointmentsManager.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsManager.Presentation.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorService _doctorService;

        public DoctorController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            try
            {
                var doctors = await _doctorService.ListDoctorsAsync();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            try
            {
                var doctor = await _doctorService.SearchDoctorAsync(id);
                return Ok(doctor);
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
        [HttpGet("schedule/{id}")]
        public async Task<IActionResult> GetDoctorSchedule(int id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorScheduleAsync(id);
                return Ok(doctor);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }
        [HttpGet("with-status/{status}")]
        public async Task<IActionResult> GetDoctorsByStatus(UserStatus status)
        {
            try
            {
                var doctors = await _doctorService.SearchByStatusAsync(status);
                return Ok(doctors);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorModel createDoctorModel)
        {
            try
            {
                var doctorDto = createDoctorModel.ToDto();
                var doctor = await _doctorService.RegisterDoctorAsync(doctorDto);
                return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorModel updateDoctorModel)
        {
            try
            {
                var doctorDto = updateDoctorModel.ToDto();
                var doctor = await _doctorService.UpdateDoctorAsync(id, doctorDto);
                return Ok(doctor);
            }
            catch (InvalidDateTimeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidEmailException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidCPFException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidRMCException ex)
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
        [HttpPut("schedule/{id}")]
        public async Task<IActionResult> UpdateDoctorSchedule(int id, [FromBody] IEnumerable<UpdateDoctorScheduleModel> updateDoctorScheduleModel)
        {
            try
            {
                var doctorDto = updateDoctorScheduleModel.Select(s => s.ToDto());
                var doctor = await _doctorService.UpdateDoctorScheduleAsync(id, doctorDto);
                return Ok(doctor);
            }
            catch (InvalidDateTimeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidEmailException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidCPFException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidRMCException ex)
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
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusModel updateStatusModel)
        {
            try
            {
                var statusDTO = updateStatusModel.ToDto();
                var doctor = await _doctorService.UpdateStatusAsync(id, statusDTO);
                return Ok(doctor);
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
