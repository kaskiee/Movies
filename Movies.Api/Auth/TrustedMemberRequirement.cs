using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Movies.Api.Auth;

public class TrustedMemberRequirement(string apiKey) : IAuthorizationHandler, IAuthorizationRequirement
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User.HasClaim(AuthConstants.AdminUserClaimName, "true") ||
            context.User.HasClaim(AuthConstants.TrustedMemberClaimName, "true"))
        {
            context.Succeed(this);
            return Task.CompletedTask;
        }

        var httpContext = context.Resource as HttpContext;
        if (httpContext is null)
        {
            return Task.CompletedTask;
        }

        if (!httpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey) || apiKey != extractedApiKey)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        // NOTE: Hardcoded for demo purposes only - same as AdminAuthRequirement
        var testUserId = "66d85b3d-035c-40f3-8b64-96c18fd6e6da";

        var identity = (ClaimsIdentity)httpContext.User.Identity!;
        identity.AddClaim(new Claim("userid", testUserId));
        context.Succeed(this);
        return Task.CompletedTask;
    }
}
