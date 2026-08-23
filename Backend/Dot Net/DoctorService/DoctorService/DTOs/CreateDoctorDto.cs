using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace DoctorService.DTOs;

public class CreateDoctorDto
{
    [Required(ErrorMessage = "UserId is required.")]
    [Range(1, long.MaxValue, ErrorMessage = "UserId must be a valid positive integer.")] public long UserId { get; set; }



    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters.")]
    public string FullName { get; set; } = string.Empty;



    [Required(ErrorMessage = "Specialization is required.")]
    [StringLength(100, ErrorMessage = "Specialization cannot exceed 100 characters.")]
    public string Specialization { get; set; } = string.Empty;



    [Required(ErrorMessage = "License number is required.")]
    [StringLength(50, ErrorMessage = "License number cannot exceed 50 characters.")]
    public string LicenseNumber { get; set; } = string.Empty;


    [Phone(ErrorMessage = "Invalid phone number format.")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
    public string Phone { get; set; } = string.Empty;

    [Range(0, 70, ErrorMessage = "Experience years must be between 0 and 70.")]
    public int ExperienceYears { get; set; }


    [Range(0.00, 10000.00, ErrorMessage = "Consultation fee must be between 0 and 10,000.")]
    public decimal ConsultationFee { get; set; }

}