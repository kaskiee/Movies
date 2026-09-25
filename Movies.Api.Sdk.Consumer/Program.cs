
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Sdk;
using Movies.Contracts.Requests;
using Refit;

var services = new ServiceCollection();

services.AddRefitClient<IMoviesApi>(x => new RefitSettings
    {
        AuthorizationHeaderValueGetter = (_,_) => ValueTask.FromResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIwODM0YjU1My0wNTc2LTQzMjgtOGFiMi1mNDA0MGY0MmY2NjciLCJzdWIiOiJrYXNpYUBrYXNpYS5wbCIsImVtYWlsIjoia2FzaWFAa2FzaWEucGwiLCJ1c2VyaWQiOiJkODU2NmRlMy1iMWE2LTRhOWItYjg0Mi04ZTM4ODdhODJlMjUiLCJhZG1pbiI6dHJ1ZSwidHJ1c3RlZF9tZW1iZXIiOnRydWUsIm5iZiI6MTc4OTkyNTk3MSwiZXhwIjoxNzg5OTU0NzcxLCJpYXQiOjE3ODk5MjU5NzEsImlzcyI6Imh0dHBzOi8vaWQua2Fza2llLnBsIiwiYXVkIjoiaHR0cHM6Ly9tb3ZpZXMua2Fza2llLnBsIn0.sO-d7m7RqAB-lA_SyfWdIzfIoAiBVZzpkgvP7icmXqw")
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