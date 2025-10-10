using Bogus;
using MedSolutions.Domain.Models;
using MedSolutions.Shared.Extensions;

namespace MedSolutions.Infrastructure.Data.Seed.Fakers;

public static class PatientPairFaker
{
    public static Faker<PatientPair> CreateFaker(Guid medicalProfileId, List<Guid> patientIds, List<short> patientPairTypeIds, string locale = "en")
    {
        return new Faker<PatientPair>(locale)
            .RuleFor(p => p.Id, _ => GuidExtensions.NewSequentialGuid())
            .RuleFor(p => p.MedicalProfileId, (f) => medicalProfileId)
            .RuleFor(p => p.PatientId, (f) => f.PickRandom(patientIds))
            .RuleFor(p => p.PairedPatientId, (f, p) => {
                List<Guid> uniqueIds = patientIds.Where(id => id != p.PatientId).ToList();
                return f.PickRandom(uniqueIds);
            })
            .RuleFor(p => p.PatientPairTypeId, (f) => f.PickRandom(patientPairTypeIds))
            ;
    }
}
