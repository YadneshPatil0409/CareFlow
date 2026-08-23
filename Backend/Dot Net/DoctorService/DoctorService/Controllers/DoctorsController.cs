using DoctorService.Data;
using DoctorService.DTOs;
using DoctorService.Exceptions;
using DoctorService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Controllers;

[ApiController]
[Route("api/v1/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly DoctorDbContext _context;

    public DoctorsController(DoctorDbContext context)
    {
        _context = context;
    }

    // POST: api/v1/doctors
    // Restricted Route: Only users with ROLE_ADMIN or ROLE_DOCTOR can create profiles
    [Authorize(Roles = "ROLE_ADMIN,ROLE_DOCTOR")]
    [HttpPost]
    public async Task<ActionResult<DoctorResponseDto>> CreateDoctor([FromBody] CreateDoctorDto dto)
    {
        if (await _context.Doctors.AnyAsync(d => d.UserId == dto.UserId))
        {
            throw new DuplicateResourceException($"Doctor profile already exists for UserId {dto.UserId}.");
        }

        var doctor = new Doctor
        {
            UserId = dto.UserId,
            FullName = dto.FullName,
            Specialization = dto.Specialization,
            LicenseNumber = dto.LicenseNumber,
            Phone = dto.Phone,
            ExperienceYears = dto.ExperienceYears,
            ConsultationFee = dto.ConsultationFee
        };

        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();

        var response = MapToResponse(doctor);
        return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, response);
    }

    // GET: api/v1/doctors/{id}
    [HttpGet("{id:long}")]
    public async Task<ActionResult<DoctorResponseDto>> GetDoctorById(long id)
    {
        var doctor = await _context.Doctors.FindAsync(id)
            ?? throw new ResourceNotFoundException($"Doctor with ID {id} was not found.");

        return Ok(MapToResponse(doctor));
    }


    /// Protected Route: Requires a valid JWT token
    [Authorize]
    [HttpGet("user/{userId:long}")]
    public async Task<ActionResult<DoctorResponseDto>> GetDoctorByUserId(long id)
    {
        //var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
        //if (doctor == null) return NotFound(new { message = "Doctor profile not found." });

        //return Ok(MapToResponse(doctor));
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == id)
            ?? throw new ResourceNotFoundException($"Doctor profile for UserId {id} was not found.");

        return Ok(MapToResponse(doctor));
    }



    private static DoctorResponseDto MapToResponse(Doctor doctor) => new()
    {
        Id = doctor.Id,
        UserId = doctor.UserId,
        FullName = doctor.FullName,
        Specialization = doctor.Specialization,
        LicenseNumber = doctor.LicenseNumber,
        Phone = doctor.Phone,
        ExperienceYears = doctor.ExperienceYears,
        ConsultationFee = doctor.ConsultationFee,
        IsActive = doctor.IsActive
    };


}