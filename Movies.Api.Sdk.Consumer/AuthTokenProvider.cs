using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using Movies.Contracts.Requests.Helpers;

namespace Movies.Api.Sdk.Consumer;

public class AuthTokenProvider(HttpClient httpClient)
{
    private string _cachedToken = string.Empty;
    private static readonly SemaphoreSlim Lock = new(1, 1);

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_cachedToken))
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_cachedToken);
            var expiryTimeText = jwt.Claims.Single(claim => claim.Type == "exp").Value;
            var expiryDateTime = UnixTimeStampToDateTime(int.Parse(expiryTimeText));
            if (expiryDateTime > DateTime.UtcNow)
            {
                return _cachedToken;
            }
        }

        await Lock.WaitAsync();
        var response = await httpClient.PostAsJsonAsync("http://localhost:5062/token", new
        {
            userId = "d8566de3-b1a6-4a9b-b842-8e3887a82e25",
            email = "kasia@kaskie.pl",
            customClaims = new Dictionary<string, object>
            {
                { "admin", true },
                { "trusted_member", true }
            }
        });
        var newToken = await response.Content.ReadAsStringAsync();
        _cachedToken = newToken;
        Lock.Release();
        return newToken;
    }

    private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}