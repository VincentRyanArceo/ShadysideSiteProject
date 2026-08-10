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

        // We mark it 'async' and return a 'Task' that promises a List of SpotifyTracks
        public async Task<List<SpotifyTrack>> GetTopTracksAsync(string token, string artistId)
        {
            // Prepare the web request to the Spotify Top Tracks endpoint
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/artists/{artistId}/top-tracks?market=US");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // AWAIT: We pause here while the internet request travels to Spotify and back.
            // The server is free to hanbdle other requests while we wait for the response.
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            // AWAIT: We pause again because reading a large data stream takes a fraction of a second
            var responseStream = await response.Content.ReadAsStreamAsync();

            // AWAIT: We pause one last time to convert (desterialize) the JSON data into a C# object we can work with.
            var result = await JsonSerializer.DeserializeAsync<SpotifyTracksResponse>(responseStream);

            //Once everything is done, we return the tracks (or an empty list if nothing was found)
            return result?.Tracks ?? new List<SpotifyTrack>();
        }

    }
}
