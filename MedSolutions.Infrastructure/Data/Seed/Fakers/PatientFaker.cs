using Bogus;
using MedSolutions.Domain.Entities;
using MedSolutions.Shared.Extensions;
using BogusGender = Bogus.DataSets.Name.Gender;
using Enums = MedSolutions.Domain.Enums;

namespace MedSolutions.Infrastructure.Data.Seed.Fakers;

public static class PatientFaker
{
    private static readonly int[] _mobileNumbers = [3, 4, 5, 7, 8, 9];
    private static readonly int[] _phoneNumbers = [0, 1, 3];

    public static string GenerateGreekMobileNumber(Faker f)
    {
        int digit = f.PickRandom(_mobileNumbers);
        string placeholder = f.Random.Replace("#######");
        return $"69{digit}{placeholder}";
    }
    public static string GenerateGreekPhoneNumber(Faker f)
    {
        int digit = f.PickRandom(_phoneNumbers);
        string placeholder = f.Random.Replace("#######");
        return $"21{digit}{placeholder}";
    }
    public static string GenerateGreekPersonalIdNumber(Faker f)
    {
        string firstPart = f.Random.AlphaNumeric(1) + f.Random.AlphaNumeric(1);

        char secondPart = f.Random.Char('A', 'Z');

        string digits = f.Random.ReplaceNumbers("#########");

        return $"{firstPart.ToUpper()}{secondPart}{digits}";
    }
    public static Faker<Patient> CreateFaker(Guid profileId, List<Enums.AppointmentType> appointmentTypeIds, string locale = "en")
    {
        return new Faker<Patient>(locale)
            .RuleFor(p => p.Id, _ => GuidExtensions.NewSequentialGuid())
            .RuleFor(p => p.MedicalProfileId, _ => profileId)
            .RuleFor(p => p.Gender, f => f.PickRandom<Enums.Gender>())
            .RuleFor(p => p.FirstName, (f, p) => f.Name.FirstName(p.Gender == Enums.Gender.Male ? BogusGender.Male : BogusGender.Female))
            .RuleFor(p => p.LastName, (f, p) => f.Name.LastName(p.Gender == Enums.Gender.Male ? BogusGender.Male : BogusGender.Female))
            .RuleFor(p => p.Patronymic, f => f.Name.FirstName(BogusGender.Male))
            .RuleFor(p => p.Referrer, f => f.Name.FullName().OrNull(f, 0.8f))
            .RuleFor(p => p.Address, f => f.Address.StreetAddress().OrNull(f, 0.3f))
            .RuleFor(p => p.City, f => f.Address.City())
            .RuleFor(p => p.Zip, f => f.Address.ZipCode().OrNull(f, 0.3f))
            .RuleFor(p => p.Latitude, f => (decimal)f.Address.Latitude())
            .RuleFor(p => p.Longitude, f => (decimal)f.Address.Longitude())
            .RuleFor(p => p.PhoneNumber, f => GenerateGreekPhoneNumber(f).OrNull(f, 0.8f))
            .RuleFor(p => p.MobileNumber, f => GenerateGreekMobileNumber(f).OrNull(f, 0.1f))
            .RuleFor(p => p.AMKA, f => f.Random.ReplaceNumbers("###########"))
            .RuleFor(p => p.PersonalIdNumber, f => GenerateGreekPersonalIdNumber(f).OrNull(f, 0.8f))
            .RuleFor(p => p.BirthDate, f => DateOnly.FromDateTime(f.Date.Past(80, DateTime.Today.AddYears(-18))))
            .RuleFor(p => p.Biopsy, f => f.Random.Bool(0.1f))
            .RuleFor(u => u.Email, f => f.Internet.Email().ToLower().OrNull(f, 0.9f))
            .FinishWith((f, p) => {
                p.Appointments = AppointmentFaker
                    .CreateFaker(p, appointmentTypeIds, locale)
                    .Generate(10);
            })
            ;
    }
}
