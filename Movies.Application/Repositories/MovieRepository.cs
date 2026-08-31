using System.Data;
using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

internal class MovieRepository(IDbConnectionFactory dbConnectionFactory) : IMovieRepository
{

    public async Task<bool> CreateAsync(Movie movie)
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
            insert into movies (id, slug, title, yearofrelease)
            values (@Id, @Slug, @Title, @YearOfRelease)
            """, movie));
        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    """
                    insert into genres (movieId, name)
                    values (@MovieId, @Name)
                    """, new { MovieId = movie.Id, Name = genre }));
            }
        }
        transaction.Commit();

        return result > 0;
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Movie?> GetBySlugAsync(string slug)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}