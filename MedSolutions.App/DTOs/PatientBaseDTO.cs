using System.ComponentModel.DataAnnotations;
using MedSolutions.Domain.Enums;
namespace MedSolutions.App.DTOs;

public class PatientBaseDTO
{
    public Guid? Id { get; set; }

    [Required]
    [Display(Name = "Medical Profile")]
    public required Guid MedicalProfileId { get; set; }

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
    public Gender Gender { get; set; }

    [Display(Name = "Date of Birth")]
    public DateOnly? BirthDate { get; set; }

    [MaxLength(100)]
    public string? Address { get; set; }

    [Required]
    [MaxLength(50)]
    public string City { get; set; } = default!;

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
    [Range(-90, 90)]
    public decimal? Latitude { get; set; }
    [Range(-180, 180)]
    public decimal? Longitude { get; set; }
}
