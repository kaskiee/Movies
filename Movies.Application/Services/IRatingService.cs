namespace Movies.Application.Services;

public interface IRatingService
{
    Task<bool> RateMovieAsync(Guid id, int rating, Guid userId, CancellationToken cancellationToken = default);
}