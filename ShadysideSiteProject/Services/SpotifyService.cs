using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ShadysideSiteProject.Services
{
    public class SpotifyService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        // The constructor automaticallu pulls in the HttpCleient and our hidden secrets (IConfiguration)
        public SpotifyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            // 1. Pull the key from local secrets.json
            var clientId = _configuration["Spotify:ClientId"];
            var clientSecret = _configuration["Spotify:ClientSecret"];

            // 2. Spotify requires keys to be combined and Base64 encoded
            var authString = $"{clientId}:{clientSecret}";
            var authBytes = Encoding.UTF8.GetBytes(authString);
            var base64Auth = Convert.ToBase64String(authBytes);

            // 3. Setup the HTTP request to ask for a token
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64Auth);
            requestMessage.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            // 4. Send the request and get the response
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            // 5. Parse the JSON response to get the token string
            var responseStream = await response.Content.ReadAsStreamAsync();
            var authResponse = await JsonSerializer.DeserializeAsync<JsonElement>(responseStream);

            return authResponse.GetProperty("access_token").GetString() ?? throw new InvalidOperationException("Access token not found in response.");

        }
    }
}
