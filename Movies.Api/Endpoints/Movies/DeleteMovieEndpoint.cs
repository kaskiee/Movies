using Movies.Api.Auth;
using Movies.Application.Services;

namespace Movies.Api.Endpoints.Movies;

public static class DeleteMovieEndpoint
{
    public const string Name = "DeleteMovie";

    public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Movies.Delete, async (
            Guid id, IMovieService movieService, 
            CancellationToken cancellationToken) =>
        {
            var deleted = await movieService.DeleteByIdAsync(id, cancellationToken);
            if (!deleted)
            {
                return Results.NotFound();
            }

            return TypedResults.Ok();
        })
        .WithName(Name)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(AuthConstants.AdminUserPolicyName);
        return app;
    }
}