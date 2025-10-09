using Microsoft.AspNetCore.Cors.Infrastructure;

namespace MedSolutions.Api.Middlewares;

public class CustomCorsPolicyProvider : ICorsPolicyProvider
{
    public Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
    {
        var policy = new CorsPolicy();

        policy.Origins.Add("*");
        policy.Methods.Add("*");
        policy.Headers.Add("*");
        policy.SupportsCredentials = true;

        return Task.FromResult(policy);
    }
}
