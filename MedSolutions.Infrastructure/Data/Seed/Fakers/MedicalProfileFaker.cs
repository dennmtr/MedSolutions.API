using Bogus;
using MedSolutions.Domain.Entities;
using MedSolutions.Shared.Extensions;
using Enums = MedSolutions.Domain.Enums;

namespace MedSolutions.Infrastructure.Data.Seed.Fakers;

public static class MedicalProfileFaker
{
    public static Faker<MedicalProfile> CreateFaker(Guid userId, List<Enums.MedicalSpecialty> medicalSpecialtyIds, List<short> patientPairTypeIds, Dictionary<short, List<short>> appointmentTypes, string? locale = "en")
    {

        Faker<MedicalProfile> faker = new Faker<MedicalProfile>(locale)
            .RuleFor(p => p.Id, _ => userId)
            .RuleFor(p => p.CompanyName, (f, p) => string.IsNullOrEmpty(p.CompanyName) ? f.Company.CompanyName() : p.CompanyName)
            .RuleFor(p => p.Tin, (f, p) => string.IsNullOrEmpty(p.Tin) ? f.Random.ReplaceNumbers("#########") : p.Tin)
            .RuleFor(p => p.Address, (f, p) => string.IsNullOrEmpty(p.Address) ? f.Address.StreetAddress() : p.Address)
            .RuleFor(p => p.City, (f, p) => string.IsNullOrEmpty(p.City) ? f.Address.City() : p.City)
            .RuleFor(p => p.Zip, (f, p) => string.IsNullOrEmpty(p.Zip) ? f.Address.ZipCode() : p.Zip)
            .RuleFor(p => p.SubscriptionStartDate, (f, p) => p.SubscriptionStartDate == DateTime.MinValue ? f.Date.Past(1).TrimToSeconds() : p.SubscriptionStartDate)
            .RuleFor(p => p.SubscriptionEndDate, (f, p) => p.SubscriptionEndDate == DateTime.MinValue ? f.Date.Future(1).TrimToSeconds() : p.SubscriptionEndDate)
            .RuleFor(p => p.MedicalSpecialtyId, (f, p) => p.MedicalSpecialtyId == null ? f.PickRandom(medicalSpecialtyIds) : p.MedicalSpecialtyId)
            .RuleFor(p => p.Comments, (f, p) => string.IsNullOrEmpty(p.Comments) ? f.Lorem.Paragraphs(f.Random.Int(1, 2)).OrNull(f, 0.1f) : p.Comments)
            .FinishWith((f, p) => {

                p.Patients = PatientFaker
                    .CreateFaker(p.Id, appointmentTypes[(short)p.MedicalSpecialtyId!], locale!)
                    .Generate(50);

                var patientIds = p.Patients
                    .Select(p => p.Id)
                    .Take(20)
                    .ToList();

                var fakePatientPairs = PatientPairFaker
                    .CreateFaker(p.Id, patientIds, patientPairTypeIds, locale)
                    .Generate(5);

                fakePatientPairs.ForEach(p => p.Normalize());

                fakePatientPairs = fakePatientPairs
                    .DistinctBy(p => (p.PatientId, p.PairedPatientId))
                    .ToList();

                p.PatientPairs = fakePatientPairs;
            })
            ;
        return faker;
    }
}
