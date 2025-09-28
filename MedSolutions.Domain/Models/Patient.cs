using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace MedSolutions.Domain.Models;

public class Patient : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(MedicalProfile))]
    public string MedicalProfileId { get; set; } = null!;

    public MedicalProfile? MedicalProfile { get; set; }

    [MaxLength(100)]
    public string FirstName { get; set; } = null!;
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [MaxLength(100)]
    public string? FirstNameLatin { get; set; }

    [MaxLength(100)]
    public string? LastNameLatin { get; set; }
    [MaxLength(100)]
    public string? Patronymic { get; set; }

    [StringLength(11, MinimumLength = 11)]
    public string? AMKA { get; set; }

    [StringLength(12, MinimumLength = 12)]
    public string? PersonalIdNumber { get; set; }

    public Enums.Gender Gender { get; set; }
    public DateOnly? BirthDate { get; set; }

    [MaxLength(100)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string City { get; set; } = null!;

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
    public bool? Biopsy { get; set; } = false;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Point Position { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<PatientPair> FirstGroupPair { get; set; } = [];
    public ICollection<PatientPair> SecondGroupPair { get; set; } = [];

}


