using AutoMapper;
using MedSolutions.App.Common.Models;
using MedSolutions.App.Common.Extensions;
using MedSolutions.App.Filters;
using MedSolutions.App.Interfaces;
using MedSolutions.App.ViewModels;
using MedSolutions.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MedSolutions.App.Logging;

namespace MedSolutions.App.Services;

public class PatientService(
    IPatientRepository patientRepository,
    IConfiguration config,
    ILogger<AuthService> logger,
    IMapper mapper) : IPatientService
{
    private readonly IConfiguration _config = config;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IPatientRepository _patientRepository = patientRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<PaginationViewModel<PatientViewModel>> GetPatientsAsync(QueryPagination? paginationProps, PatientQueryFilter? filterProps, PatientQuerySorting? sortingProps, CancellationToken cancellationToken)
    {
        try
        {
            var query = _patientRepository
                .Query()
                .ApplyFilter(filterProps)
                ;
            var totalRecords = await query.LongCountAsync(cancellationToken);
            paginationProps ??= new QueryPagination();
            query = query
                .ApplySorting(sortingProps)
                .Paginate(paginationProps);
            var patients = await _mapper.ProjectTo<PatientViewModel>(
                query
                ).ToListAsync(cancellationToken)
                ;
            return paginationProps.ToViewModel(patients, totalRecords);
        }
        catch (Exception ex)
        {
            _logger.ErrorGettingPatients(ex);
            throw;
        }

    }

}
