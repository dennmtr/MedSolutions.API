using MedSolutions.App.Common.Models;
using MedSolutions.App.Filters;
using MedSolutions.App.ViewModels;

namespace MedSolutions.App.Interfaces;

public interface IPatientService
{
    Task<PaginationViewModel<PatientViewModel>> GetPatientsAsync(QueryPagination? paginationProps, PatientQueryFilter? filterProps, PatientQuerySorting? sortingProps, CancellationToken cancellationToken);
}
