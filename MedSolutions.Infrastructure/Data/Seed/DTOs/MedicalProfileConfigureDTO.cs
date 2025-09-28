using System.ComponentModel.DataAnnotations;
using Enums = MedSolutions.Domain.Enums;

namespace MedSolutions.Infrastructure.Data.Seed.DTOs;

public class MedicalProfileConfigureDTO
{
    [Required]
    [StringLength(50)]
    public required string CompanyName { get; set; }

    [Required]
    [StringLength(50)]
    public required string Tin { get; set; }

    [Required]
    [StringLength(100)]
    public required string Address { get; set; }

    [Required]
    [StringLength(50)]
    public required string City { get; set; }

    [Required]
    [StringLength(50)]
    public required string Zip { get; set; }

    [Required]
    public Enums.MedicalSpecialty MedicalSpecialtyId { get; set; }

}
