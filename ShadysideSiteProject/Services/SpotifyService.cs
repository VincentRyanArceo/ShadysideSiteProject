using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ShadysideSiteProject.Models;

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

        // We changed the parameter from artistId to bandName
        public async Task<List<SpotifyTrack>> GetTopTracksAsync(string token, string bandName)
        {
            // 1. Prepare the web request using the Spotify Search endpoint
            // We format the query to specifically search for tracks by the artist
            string query = Uri.EscapeDataString($"artist:{bandName}");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/search?q={query}&type=track&limit=10");

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 2. Send the request
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            // 3. Read the stream and deserialize into our new SpotifySearchResponse wrapper
            var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<SpotifySearchResponse>(responseStream);

            // 4. Navigate through the nested objects to return the list of tracks
            return result?.Tracks?.Items ?? new List<SpotifyTrack>();
        }

    }
}
