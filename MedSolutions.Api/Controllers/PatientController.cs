using MedSolutions.App.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSolutions.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PatientController() : ControllerBase
{
    [HttpPost()]
    public async Task<IActionResult> CreatePatient([FromBody] PatientBaseDTO patient) => NoContent();

}



