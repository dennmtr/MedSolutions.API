namespace MedSolutions.App.ViewModels;
public class PatientViewModel
{
    public required int Id { get; set; }
    public required string MedicalProfileId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Patronymic { get; set; }
    public string? AMKA { get; set; }
    public string? PersonalIdNumber { get; set; }
    public required Domain.Enums.Gender Gender { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Address { get; set; }
    public required string City { get; set; }
    public string? Zip { get; set; }
    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }
    public string? Comments { get; set; }
    public string? Referrer { get; set; }
    public bool? Biopsy { get; set; } = false;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
