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

    // =========================================================
    // GET: api/v1/doctors
    // Get all active doctors
    // =========================================================
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorResponseDto>>> GetAllDoctors()
    {
        var doctors = await _context.Doctors
            .Where(d => d.IsActive)
            .ToListAsync();

        return Ok(doctors.Select(MapToResponse));
    }


    // =========================================================
    // GET: api/v1/doctors/{id}
    // Get doctor by Doctor ID
    // =========================================================
    [HttpGet("{id:long}")]
    public async Task<ActionResult<DoctorResponseDto>> GetDoctorById(long id)
    {
        var doctor = await _context.Doctors.FindAsync(id)
            ?? throw new ResourceNotFoundException(
                $"Doctor with ID {id} was not found."
            );

        return Ok(MapToResponse(doctor));
    }


    // =========================================================
    // GET: api/v1/doctors/user/{userId}
    // Get doctor profile using IdentityService User ID
    // =========================================================
    [Authorize]
    [HttpGet("user/{userId:long}")]
    public async Task<ActionResult<DoctorResponseDto>> GetDoctorByUserId(long userId)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.UserId == userId)
            ?? throw new ResourceNotFoundException(
                $"Doctor profile for User ID {userId} was not found."
            );

        return Ok(MapToResponse(doctor));
    }


    // =========================================================
    // POST: api/v1/doctors
    // Create a new doctor profile
    // Only ADMIN or DOCTOR can create
    // =========================================================
    [Authorize(Roles = "ROLE_ADMIN,ROLE_DOCTOR")]
    [HttpPost]
    public async Task<ActionResult<DoctorResponseDto>> CreateDoctor(
        [FromBody] CreateDoctorDto dto)
    {
        // Check whether a doctor profile already exists
        // for this IdentityService UserId
        bool exists = await _context.Doctors
            .AnyAsync(d => d.UserId == dto.UserId);

        if (exists)
        {
            throw new DuplicateResourceException(
                $"Doctor profile already exists for User ID {dto.UserId}."
            );
        }

        var doctor = new Doctor
        {
            UserId = dto.UserId,
            FullName = dto.FullName,
            Specialization = dto.Specialization,
            LicenseNumber = dto.LicenseNumber,
            Phone = dto.Phone,
            ExperienceYears = dto.ExperienceYears,
            ConsultationFee = dto.ConsultationFee,

            // Newly created doctor is active
            IsActive = true
        };

        _context.Doctors.Add(doctor);

        await _context.SaveChangesAsync();

        var response = MapToResponse(doctor);

        return CreatedAtAction(
            nameof(GetDoctorById),
            new { id = doctor.Id },
            response
        );
    }


    // =========================================================
    // PATCH: api/v1/doctors/{id}/deactivate
    // Admin can revoke/deactivate a doctor
    // =========================================================
    [Authorize(Roles = "ROLE_ADMIN")]
    [HttpPatch("{id:long}/deactivate")]
    public async Task<ActionResult<DoctorResponseDto>> DeactivateDoctor(long id)
    {
        var doctor = await _context.Doctors.FindAsync(id)
            ?? throw new ResourceNotFoundException(
                $"Doctor with ID {id} was not found."
            );

        if (!doctor.IsActive)
        {
            return Ok(MapToResponse(doctor));
        }

        doctor.IsActive = false;

        await _context.SaveChangesAsync();

        return Ok(MapToResponse(doctor));
    }


    // =========================================================
    // Mapping Entity -> Response DTO
    // =========================================================
    private static DoctorResponseDto MapToResponse(Doctor doctor)
    {
        return new DoctorResponseDto
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
}