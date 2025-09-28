using Bogus;
using MedSolutions.Domain.Models;
using MedSolutions.Shared.Extensions;
using Enums = MedSolutions.Domain.Enums;

namespace MedSolutions.Infrastructure.Data.Seed.Fakers;

public static class MedicalProfileFaker
{
    public static Faker<MedicalProfile> CreateFaker(User user, List<Enums.MedicalSpecialty> medicalSpecialtyIds, string? locale = "en")
    {

        Faker<MedicalProfile> faker = new Faker<MedicalProfile>(locale)
            .RuleFor(p => p.Id, () => user.Id)
            .RuleFor(p => p.CompanyName, f => f.Company.CompanyName())
            .RuleFor(p => p.Tin, f => f.Random.ReplaceNumbers("#########"))
            .RuleFor(p => p.Address, f => f.Address.StreetAddress())
            .RuleFor(p => p.City, f => f.Address.City())
            .RuleFor(p => p.Zip, f => f.Address.ZipCode())
            .RuleFor(p => p.SubscriptionStartDate, f => f.Date.Past(1).TrimToSeconds())
            .RuleFor(p => p.SubscriptionEndDate, f => f.Date.Future(1))
            .RuleFor(p => p.MedicalSpecialtyId, f => f.PickRandom(medicalSpecialtyIds))
            .RuleFor(p => p.Comments, f => f.Lorem.Paragraphs(f.Random.Int(1, 2)).OrNull(f, 0.1f))
            ;
        return faker;
    }
}
