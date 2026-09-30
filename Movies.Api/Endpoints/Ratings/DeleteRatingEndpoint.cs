using Movies.Api.Auth;
using Movies.Application.Services;

namespace Movies.Api.Endpoints.Ratings;

public static class DeleteRatingEndpoint
{
    public const string Name = "DeleteRating";

    public static IEndpointRouteBuilder MapDeleteRating(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Movies.DeleteRating, async (
                Guid movieId, IRatingService ratingService,
                HttpContext context, CancellationToken cancellationToken) =>
            {
                var userId = context.GetUserId();
                var result = await ratingService.DeleteRatingAsync(movieId, userId!.Value,
                    cancellationToken);
                if (!result)
                {
                    return Results.NotFound();
                }

                return TypedResults.Ok();
            })
            .WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();
        return app;
    }
}
