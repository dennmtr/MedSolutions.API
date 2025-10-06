using System.ComponentModel.DataAnnotations;

namespace MedSolutions.App.DTOs;

public class PatientBaseDTO
{
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Medical Profile")]
    public required string MedicalProfileId { get; set; }

    [Required]
    [MaxLength(100)]
    [Display(Name = "Name")]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    [Display(Name = "Family Name")]
    public required string LastName { get; set; }

    [MaxLength(100)]
    [Display(Name = "Father's Name")]
    public string? Patronymic { get; set; }

    [StringLength(11, MinimumLength = 11)]
    public string? AMKA { get; set; }

    [StringLength(12)]
    [Display(Name = "Personal ID Number")]
    public string? PersonalIdNumber { get; set; }

    [Required]
    public Domain.Enums.Gender Gender { get; set; }

    [Display(Name = "Date of Birth")]
    public DateOnly? BirthDate { get; set; }

    [MaxLength(100)]
    public string? Address { get; set; }

    [Required]
    [MaxLength(50)]
    public string City { get; set; } = null!;

    [MaxLength(50)]
    public string? Zip { get; set; }

    [MaxLength(20)]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [MaxLength(20)]
    [Display(Name = "Mobile Number")]
    public string? MobileNumber { get; set; }

    [EmailAddress]
    [MaxLength(254)]
    [Display(Name = "Email Address")]
    public string? Email { get; set; }

    public string? Comments { get; set; }

    [MaxLength(255)]
    public string? Referrer { get; set; }
    public bool? Biopsy { get; set; } = false;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
