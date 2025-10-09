using MedSolutions.Domain.Interfaces;
using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Common.Repositories;
using MedSolutions.Infrastructure.Data;

namespace MedSolutions.Infrastructure.Repositories;
public class PatientRepository(MedSolutionsDbContext context) : RepositoryBase<Patient>(context), IPatientRepository
{
}

