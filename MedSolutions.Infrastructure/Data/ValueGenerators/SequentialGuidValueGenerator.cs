using MedSolutions.Shared.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace MedSolutions.Infrastructure.Data.ValueGenerators;
public class SequentialGuidValueGenerator : ValueGenerator<Guid>
{
    public override bool GeneratesTemporaryValues => false;

    public override Guid Next(EntityEntry entry) => GuidExtensions.NewSequentialGuid();
}
