using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedSolutions.Domain.Common.Models;

namespace MedSolutions.Domain.Models;

public class PatientPair : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    [ForeignKey(nameof(MedicalProfile))]
    public Guid MedicalProfileId { get; set; } = default!;
    public MedicalProfile? MedicalProfile { get; set; }
    [ForeignKey(nameof(Patient))]
    public Guid PatientId { get; set; }
    public Patient? Patient { get; set; }
    [ForeignKey(nameof(PairedPatient))]
    public Guid PairedPatientId { get; set; }
    public Patient? PairedPatient { get; set; }
    [ForeignKey(nameof(PatientPairType))]
    public short PatientPairTypeId { get; set; } = 1;
    public PatientPairType? PatientPairType { get; set; }

    /// Ensures the patient pair is stored correctly to avoid duplicates.
    /// Example: if patient A is the parent and B is the child, but another record lists
    /// B as the parent and A as the child, Normalize() will reorder the pair to (Parent=A, Child=B).
    public void Normalize()
    {
        if (PatientId > PairedPatientId)
        {
            (PatientId, PairedPatientId) = (PairedPatientId, PatientId);
        }
    }
}


