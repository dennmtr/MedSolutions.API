using System.Security.Claims;
using MedSolutions.App.Interfaces;

namespace MedSolutions.Api.Services;

public class CurrentMedicalProfileService(IHttpContextAccessor httpContextAccessor) : ICurrentMedicalProfileService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid? MedicalProfileId
    {
        get {
            var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(claimValue, out var guid) ? guid : null;
        }
    }
}
