using System.Security.Claims;
using MedSolutions.App.Interfaces;

namespace MedSolutions.Api.Providers;

public class AppContextProvider(IHttpContextAccessor httpContextAccessor) : IAppContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid? CurrentMedicalProfileId
    {
        get {
            var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(claimValue, out var guid) ? guid : null;
        }
    }

    public bool IsVisibilityModeEnabled =>
            //throw new NotImplementedException();
            false;
}
