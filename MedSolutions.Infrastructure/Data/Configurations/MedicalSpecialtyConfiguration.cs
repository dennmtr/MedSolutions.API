using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Enums = MedSolutions.Domain.Enums;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class MedicalSpecialtyConfiguration(DbProviderInfo dbProviderInfo) : IEntityTypeConfiguration<MedicalSpecialty>
{
    private readonly DbProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<MedicalSpecialty> builder)
    {

        builder.HasData(
            new MedicalSpecialty { Id = Enums.MedicalSpecialty.Unspecified, Description = "Unspecified", BusinessId = "spec.generic", DisplayOrder = 10 },
            new MedicalSpecialty { Id = Enums.MedicalSpecialty.Dermatology, Description = "Dermatology", BusinessId = "spec.dermatology", DisplayOrder = 20 }
        );
    }
}
