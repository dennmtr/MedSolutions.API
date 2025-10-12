using MedSolutions.Domain.Entities;
using MedSolutions.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Type = MedSolutions.Domain.Enums.PatientPairType;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class PatientPairTypeConfiguration(DatabaseProviderInfo dbProviderInfo) : IEntityTypeConfiguration<PatientPairType>
{
    private readonly DatabaseProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<PatientPairType> builder)
    {
        builder.HasData(
            new PatientPairType { Id = Type.Other, Description = "Other", BusinessId = "pair.other", DisplayOrder = 10 },
            new PatientPairType { Id = Type.Relative, Description = "Relative", BusinessId = "pair.relative", DisplayOrder = 20 },
            new PatientPairType { Id = Type.Spouse, Description = "Spouse", BusinessId = "pair.spouse", DisplayOrder = 30 },
            new PatientPairType { Id = Type.Child, Description = "Child", BusinessId = "pair.child", DisplayOrder = 40 },
            new PatientPairType { Id = Type.Niece, Description = "Niece", BusinessId = "pair.niece", DisplayOrder = 50 },
            new PatientPairType { Id = Type.Sibling, Description = "Sibling", BusinessId = "pair.sibling", DisplayOrder = 60 },
            new PatientPairType { Id = Type.Cousin, Description = "Cousin", BusinessId = "pair.cousin", DisplayOrder = 70 },
            new PatientPairType { Id = Type.Friend, Description = "Friend", BusinessId = "pair.friend", DisplayOrder = 80 }
        );
    }
}
