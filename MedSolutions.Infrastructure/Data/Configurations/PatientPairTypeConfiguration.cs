using MedSolutions.Domain.Entities;
using MedSolutions.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class PatientPairTypeConfiguration(DbProviderInfo dbProviderInfo) : IEntityTypeConfiguration<PatientPairType>
{
    private readonly DbProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<PatientPairType> builder)
    {
        builder.HasData(
            new PatientPairType { Id = 1, Description = "Other", BusinessId = "pair.other", DisplayOrder = 10 },
            new PatientPairType { Id = 2, Description = "Relative", BusinessId = "pair.relative", DisplayOrder = 20 },
            new PatientPairType { Id = 3, Description = "Spouse", BusinessId = "pair.spouse", DisplayOrder = 30 },
            new PatientPairType { Id = 4, Description = "Child", BusinessId = "pair.child", DisplayOrder = 40 },
            new PatientPairType { Id = 5, Description = "Niece", BusinessId = "pair.niece", DisplayOrder = 50 },
            new PatientPairType { Id = 6, Description = "Sibling", BusinessId = "pair.sibling", DisplayOrder = 60 },
            new PatientPairType { Id = 7, Description = "Cousin", BusinessId = "pair.cousin", DisplayOrder = 70 },
            new PatientPairType { Id = 8, Description = "Friend", BusinessId = "pair.friend", DisplayOrder = 80 }
        );
    }
}
