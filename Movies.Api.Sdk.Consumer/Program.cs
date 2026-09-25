
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