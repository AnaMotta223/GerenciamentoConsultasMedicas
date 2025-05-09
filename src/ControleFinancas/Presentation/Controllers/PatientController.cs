﻿using AppointmentsManager.Application.Services;
using AppointmentsManager.Domain.Enums;
using AppointmentsManager.Domain.Exceptions;
using AppointmentsManager.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsManager.Presentation.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            try
            {
                var patients = await _patientService.ListPatientsAsync();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno no servidor.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            try
            {
                var patient = await _patientService.SearchPatientAsync(id);
                return Ok(patient);
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
        [HttpGet("with-status/{status}")]
        public async Task<IActionResult> GetPatientsByStatus(UserStatus status)
        {
            try
            {
                var patients = await _patientService.SearchByStatusAsync(status);
                return Ok(patients);
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
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientModel createPatientModel)
        {
            try
            {
                var patientDto = createPatientModel.ToDto();
                var patient = await _patientService.RegisterPatientAsync(patientDto);
                return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
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
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientModel updatePatientModel)
        {
            try
            {
                var patientDto = updatePatientModel.ToDto();
                var patient = await _patientService.UpdatePatientAsync(id, patientDto);
                return Ok(patient);
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
                var patient = await _patientService.UpdateStatusAsync(id, statusDTO);
                return Ok(patient);
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
