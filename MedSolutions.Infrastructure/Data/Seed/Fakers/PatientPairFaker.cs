using Bogus;
using MedSolutions.Domain.Models;

namespace MedSolutions.Infrastructure.Data.Seed.Fakers;

public static class PatientPairFaker
{
    public static Faker<PatientPair> CreateFaker(string medicalProfileId, List<int> patientIds, List<short> patientPairTypeIds, string locale = "en")
    {
        return new Faker<PatientPair>(locale)
            .RuleFor(p => p.MedicalProfileId, (f) => medicalProfileId)
            .RuleFor(p => p.PatientId, (f, p) => f.PickRandom(patientIds))
            .RuleFor(p => p.PairedPatientId, (f, p) => {
                List<int> uniqueIds = patientIds.Where(id => id != p.PatientId).ToList();
                return f.PickRandom(uniqueIds);
            })
            .RuleFor(p => p.PatientPairTypeId, (f) => f.PickRandom(patientPairTypeIds))
            ;
    }
}
