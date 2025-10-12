using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedSolutions.Domain.Common.Entities;
using NetTopologySuite.Geometries;

namespace MedSolutions.Domain.Entities;

public class Patient : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(MedicalProfile))]
    public Guid MedicalProfileId { get; set; } = default!;

    public MedicalProfile? MedicalProfile { get; set; }

    [MaxLength(100)]
    public string FirstName { get; set; } = default!;
    [MaxLength(100)]
    public string LastName { get; set; } = default!;

    [MaxLength(100)]
    public string? FirstNameLatin { get; set; }

    [MaxLength(100)]
    public string? LastNameLatin { get; set; }
    [MaxLength(100)]
    public string? Patronymic { get; set; }

    [MaxLength(11)]
    [StringLength(11, MinimumLength = 11)]
    public string? AMKA { get; set; }

    [MaxLength(12)]
    [StringLength(12, MinimumLength = 12)]
    public string? PersonalIdNumber { get; set; }

    public Enums.Gender Gender { get; set; }
    public DateOnly? BirthDate { get; set; }

    [MaxLength(100)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string City { get; set; } = default!;

    [MaxLength(50)]
    public string? CityLatin { get; set; }

    [MaxLength(50)]
    public string? Zip { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(20)]
    public string? MobileNumber { get; set; }

    [EmailAddress]
    [MaxLength(254)]
    public string? Email { get; set; }
    public string? Comments { get; set; }
    [MaxLength(255)]
    public string? Referrer { get; set; }
    public bool Biopsy { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public Point Position { get; set; } = default!;
    public ICollection<Appointment> Appointments { get; set; } = [];

}


