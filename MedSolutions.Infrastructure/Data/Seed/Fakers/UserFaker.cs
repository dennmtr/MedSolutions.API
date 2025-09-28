using Bogus;
using MedSolutions.Domain.Models;
using Enums = MedSolutions.Domain.Enums;

namespace MedSolutions.Infrastructure.Data.Seed.Fakers;

public static class UserFaker
{
    public static Faker<User> CreateFaker(List<Enums.MedicalSpecialty> medicalSpecialtyIds, string? locale = "en")
    {

        Faker<User> faker = new Faker<User>(locale)
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, f => f.Internet.Email().ToLower())
            .RuleFor(u => u.UserName, (f, u) => u.Email)
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.MobileNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.IsActive, f => f.Random.Bool(0.9f))
            .FinishWith((f, u) => {
                u.MedicalProfile = MedicalProfileFaker
                    .CreateFaker(u, medicalSpecialtyIds, locale)
                    .Generate(1)
                    .First();
            })
            ;
        return faker;
    }
}
