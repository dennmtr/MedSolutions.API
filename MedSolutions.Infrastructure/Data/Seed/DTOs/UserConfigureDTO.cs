using System.ComponentModel.DataAnnotations;
using MedSolutions.Shared.Extensions;

namespace MedSolutions.Infrastructure.Data.Seed.DTOs;

public class UserConfigureDTO
{
    public Guid Id { get; set; } = GuidExtensions.NewSequentialGuid();

    [Required]
    [StringLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public required string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public required string Email { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(20)]
    public string? MobileNumber { get; set; }

    [Required]
    [MinLength(6)]
    public required string Password { get; set; }

    public MedicalProfileConfigureDTO? MedicalProfile { get; set; }

}
