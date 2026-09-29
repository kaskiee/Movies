using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public const string Name = "CreateMovie";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Movies.Update, async (
                Guid id, UpdateMovieRequest request, IMovieService movieService,
                HttpContext context, CancellationToken cancellationToken) =>
            {
                var userId = context.GetUserId();
        
                var movie = request.MapToMovie(id);
                var updatedMovie = await movieService.UpdateAsync(movie, userId, cancellationToken);
                if (updatedMovie is null)
                {
                    return Results.NotFound();
                }
                var response = movie.MapToResponse();
        
                return TypedResults.Ok(response);
            })
            .WithName(Name);
        return app;
    }
}