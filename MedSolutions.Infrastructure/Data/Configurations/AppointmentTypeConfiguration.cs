using MedSolutions.Domain.Entities;
using MedSolutions.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Enums = MedSolutions.Domain.Enums;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class AppointmentTypeConfiguration(DbProviderInfo dbProviderInfo) : IEntityTypeConfiguration<AppointmentType>
{
    private readonly DbProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<AppointmentType> builder)
    {
        builder.HasIndex(t => new { t.MedicalSpecialtyId, t.DisplayOrder })
        .IsUnique();

        builder.HasData(
            new AppointmentType { Id = 1, MedicalSpecialtyId = Enums.MedicalSpecialty.Unspecified, Description = "Clinical Examination", BusinessId = "appointment.common.clinical_examination", DisplayOrder = 10 },
            new AppointmentType { Id = 2, MedicalSpecialtyId = Enums.MedicalSpecialty.Dermatology, Description = "Aesthetic Procedure", BusinessId = "appointment.dermatology.aesthetic_procedure", DisplayOrder = 20 },
            new AppointmentType { Id = 3, MedicalSpecialtyId = Enums.MedicalSpecialty.Dermatology, Description = "Laser", BusinessId = "appointment.dermatology.laser", DisplayOrder = 30 }
        );
    }
}
