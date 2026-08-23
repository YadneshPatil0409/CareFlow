using System.Runtime.InteropServices;

namespace DoctorService.Models;

public class Doctor
{
    //public Guid Id { get; set; } = Guid.NewGuid();
    public long Id { get; set; } // Auto-increment BIGINT

    // Links to the userId from IdentityService
    public long UserId { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public decimal ConsultationFee { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}