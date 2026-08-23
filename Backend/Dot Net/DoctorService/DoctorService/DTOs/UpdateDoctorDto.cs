using System.ComponentModel.DataAnnotations;

namespace DoctorService.DTOs;

public class UpdateDoctorDto
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Specialization is required.")]
    public string Specialization { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string Phone { get; set; } = string.Empty;

    [Range(0, 70, ErrorMessage = "Experience years must be between 0 and 70.")]
    public int ExperienceYears { get; set; }

    [Range(0.00, 10000.00, ErrorMessage = "Consultation fee must be between 0 and 10,000.")]
    public decimal ConsultationFee { get; set; }
}