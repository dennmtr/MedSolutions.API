using Bogus;
using MedSolutions.Domain.Models;
using MedSolutions.Shared.Extensions;
using Enums = MedSolutions.Domain.Enums;

namespace MedSolutions.Infrastructure.Data.Seed.Fakers;

public static class AppointmentFaker
{
    public static Faker<Appointment> CreateFaker(Patient patient, List<short> appointmentTypeIds, string locale = "en")
    {
        DateTime[] specialDates =
        [
            DateTime.UtcNow.AddDays(-2),
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(2)
        ];

        return new Faker<Appointment>(locale)
            .RuleFor(a => a.Id, _ => GuidExtensions.NewSequentialGuid())
            .RuleFor(a => a.MedicalProfileId, _ => patient.MedicalProfileId)
            .RuleFor(a => a.PatientId, _ => patient.Id)
            .RuleFor(a => a.AppointmentTypeId, (f) => f.PickRandom(appointmentTypeIds))
            .RuleFor(a => a.Date, f => {
                DateTime value;

                if (f.Random.Bool(0.2f))
                {
                    DateTime baseDate = f.PickRandom(specialDates).Date;
                    TimeSpan randomTime = TimeSpan.FromSeconds(f.Random.Int(0, (24 * 60 * 60) - 1));
                    value = baseDate.Add(randomTime);
                    return value.TrimToSeconds();
                }
                DateTime start = DateTime.UtcNow.AddYears(-1);
                DateTime end = DateTime.UtcNow.AddDays(7);
                value = f.Date.Between(start, end);

                return value.TrimToSeconds();
            })
            .RuleFor(a => a.Comments, f => f.Lorem.Paragraphs(f.Random.Int(1, 2)).OrNull(f, 0.8f))
            .RuleFor(a => a.State, f => f.PickRandom<Enums.State>().OrNull(f, 0.9f))
            .RuleFor(a => a.Priority, f => f.PickRandom<Enums.Priority>().OrNull(f, 0.9f))
            ;
    }
}
