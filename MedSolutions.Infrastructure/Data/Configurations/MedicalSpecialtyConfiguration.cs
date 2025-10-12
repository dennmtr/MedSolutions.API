using MedSolutions.Domain.Entities;
using MedSolutions.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Specialty = MedSolutions.Domain.Enums.MedicalSpecialty;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class MedicalSpecialtyConfiguration(DatabaseProviderInfo dbProviderInfo) : IEntityTypeConfiguration<MedicalSpecialty>
{
    private readonly DatabaseProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<MedicalSpecialty> builder)
    {

        builder.HasData(
            new MedicalSpecialty { Id = Specialty.Unspecified, Description = "Unspecified", BusinessId = "spec.generic", DisplayOrder = 10 },
            new MedicalSpecialty { Id = Specialty.Dermatology, Description = "Dermatology", BusinessId = "spec.dermatology", DisplayOrder = 20 }
        );

    }
}
