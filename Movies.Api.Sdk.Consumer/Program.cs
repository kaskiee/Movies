
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Sdk;
using Movies.Api.Sdk.Consumer;
using Movies.Contracts.Requests;
using Refit;

var services = new ServiceCollection();

services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IMoviesApi>(s => new RefitSettings
    {
        AuthorizationHeaderValueGetter = async (_,_) => await s.GetRequiredService<AuthTokenProvider>().GetTokenAsync()
    })
    .ConfigureHttpClient(x =>
        x.BaseAddress = new  Uri("http://localhost:5062/"));

var provider = services.BuildServiceProvider();
var moviesApi = provider.GetRequiredService<IMoviesApi>();

var movie = await moviesApi.GetMovieAsync("017874dc-c85f-430c-9067-3720e268890d");
Console.WriteLine(JsonSerializer.Serialize(movie));

var request = new GetAllMoviesRequest
{
    Title = null,
    YearOfRelease = null,
    Page = 1,
    PageSize = 15,
    SortBy = null
};
var movies = await moviesApi.GetAllMoviesAsync(request);

foreach (var movieResponse in movies.Items)
{
    Console.WriteLine(JsonSerializer.Serialize(movieResponse));
}

var createRequest = new CreateMovieRequest
{
    Title = "The Big Lebowski",
    YearOfRelease = 1998,
    Genres = ["Comedy", "Crime"]
};
var createdMovie = await moviesApi.CreateMovieAsync(createRequest);
Console.WriteLine(JsonSerializer.Serialize(createdMovie));

var updateRequest = new UpdateMovieRequest
{
    Title = createdMovie.Title,
    YearOfRelease = createdMovie.YearOfRelease,
    Genres = createdMovie.Genres
};
var updatedMovie = await moviesApi.UpdateMovieAsync(createdMovie.Id, updateRequest);
Console.WriteLine(JsonSerializer.Serialize(updatedMovie));

await moviesApi.RateMovieAsync(createdMovie.Id, new RateMovieRequest { Rating = 5 });

var userRatings = await moviesApi.GetUserRatingsAsync();
foreach (var rating in userRatings)
{
    Console.WriteLine(JsonSerializer.Serialize(rating));
}

await moviesApi.DeleteRatingAsync(createdMovie.Id);
await moviesApi.DeleteMovieAsync(createdMovie.Id);