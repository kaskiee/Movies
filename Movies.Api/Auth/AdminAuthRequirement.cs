using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Movies.Api.Auth;

public class AdminAuthRequirement(string apiKey) :IAuthorizationHandler, IAuthorizationRequirement
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User.HasClaim(AuthConstants.AdminUserClaimName, "true"))
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
        // NOTE: Hardcoded for demo purposes only.
        // In production the userId would come from a real identity source - e.g. an API key
        // looked up (hashed) in the database with an associated owner, or from validated
        // JWT claims issued by an identity provider. Here I simulate it to show that
        // multiple auth schemes can share the same authorization logic.
        var testUserId = "66d85b3d-035c-40f3-8b64-96c18fd6e6da";

        var identity = (ClaimsIdentity)httpContext.User.Identity!;
        identity.AddClaim(new Claim("userid", testUserId));
        context.Succeed(this);
        return Task.CompletedTask;
    }
}