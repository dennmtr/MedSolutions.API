using MedSolutions.Domain.Entities;
using MedSolutions.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Specialty = MedSolutions.Domain.Enums.MedicalSpecialty;
using Type = MedSolutions.Domain.Enums.AppointmentType;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class AppointmentTypeConfiguration(DatabaseProviderInfo dbProviderInfo) : IEntityTypeConfiguration<AppointmentType>
{
    private readonly DatabaseProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<AppointmentType> builder)
    {
        builder.HasIndex(t => new { t.MedicalSpecialtyId, t.DisplayOrder })
        .IsUnique();

        builder.HasData(
            new AppointmentType { Id = Type.ClinicalExamination, MedicalSpecialtyId = Specialty.Unspecified, Description = "Clinical Examination", BusinessId = "appointment.common.clinical_examination", DisplayOrder = 10 },
            new AppointmentType { Id = Type.AestheticProcedure, MedicalSpecialtyId = Specialty.Dermatology, Description = "Aesthetic Procedure", BusinessId = "appointment.dermatology.aesthetic_procedure", DisplayOrder = 20 },
            new AppointmentType { Id = Type.Laser, MedicalSpecialtyId = Specialty.Dermatology, Description = "Laser", BusinessId = "appointment.dermatology.laser", DisplayOrder = 30 }
        );

    }
}
