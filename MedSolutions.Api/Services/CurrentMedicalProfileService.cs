using System.Security.Claims;
using MedSolutions.App.Interfaces;

namespace MedSolutions.Api.Services;

public class CurrentMedicalProfileService(IHttpContextAccessor httpContextAccessor) : ICurrentMedicalProfileService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? MedicalProfileId =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
