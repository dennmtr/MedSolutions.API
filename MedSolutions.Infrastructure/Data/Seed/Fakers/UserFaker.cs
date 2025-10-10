using Bogus;
using MedSolutions.Domain.Models;
using MedSolutions.Shared.Extensions;
using Enums = MedSolutions.Domain.Enums;

namespace MedSolutions.Infrastructure.Data.Seed.Fakers;

public static class UserFaker
{
    public static Faker<User> CreateFaker(List<Enums.MedicalSpecialty> medicalSpecialtyIds, List<short> patientPairTypeIds, Dictionary<short, List<short>> appointmentTypes, string? locale = "en")
    {

        Faker<User> faker = new Faker<User>(locale)
            .RuleFor(u => u.Id, (_, u) => u.Id == Guid.Empty ? GuidExtensions.NewSequentialGuid() : u.Id)
            .RuleFor(u => u.FirstName, (f, u) => string.IsNullOrEmpty(u.FirstName) ? f.Name.FirstName() : u.FirstName)
            .RuleFor(u => u.LastName, (f, u) => string.IsNullOrEmpty(u.LastName) ? f.Name.LastName() : u.LastName)
            .RuleFor(u => u.Email, (f, u) => string.IsNullOrEmpty(u.Email) ? f.Internet.Email().ToLower() : u.Email)
            .RuleFor(u => u.UserName, (f, u) => string.IsNullOrEmpty(u.UserName) ? u.Email : u.UserName)
            .RuleFor(u => u.PhoneNumber, (f, u) => string.IsNullOrEmpty(u.PhoneNumber) ? f.Phone.PhoneNumber() : u.PhoneNumber)
            .RuleFor(u => u.MobileNumber, (f, u) => string.IsNullOrEmpty(u.MobileNumber) ? f.Phone.PhoneNumber() : u.MobileNumber)
            .RuleFor(u => u.IsActive, (f, u) => u.IsActive == null ? f.Random.Bool(0.9f) : u.IsActive)
            .FinishWith((f, u) => {
                var faker = MedicalProfileFaker
                    .CreateFaker(u.Id, medicalSpecialtyIds, patientPairTypeIds, appointmentTypes, locale);
                if (u.MedicalProfile != null)
                {
                    faker.Populate(u.MedicalProfile);
                }
                else
                {
                    u.MedicalProfile = faker
                        .Generate(1)
                        .First();
                }
            })
            ;
        return faker;
    }
}
