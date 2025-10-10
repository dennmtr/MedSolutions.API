using MedSolutions.App.Common.Models;
using MedSolutions.App.Filters;
using MedSolutions.App.Interfaces;
using MedSolutions.App.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSolutions.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PatientsController(IPatientService patientService) : ControllerBase
{
    private readonly IPatientService _patientService = patientService;

    [HttpGet()]
    public async Task<ActionResult<PaginationViewModel<PatientViewModel>>> GetPatients([FromQuery] QueryPagination? pagination, [FromQuery] PatientQueryFilter? filters, [FromQuery] PatientQuerySorting? sorting, CancellationToken cancellationToken) => Ok(await _patientService.GetPatientsAsync(pagination, filters, sorting, cancellationToken));

}
