using ShadysideSiteProject.Services;
using System.Text.Json.Serialization;

namespace ShadysideSiteProject.Models
{
    // 1. This represents the outmost JSON object Spotify sends back
    public class  SpotifyTracksResponse
    {
        [JsonPropertyName("tracks")]
        public List<SpotifyTrack> Tracks { get; set; } = new List<SpotifyTrack>();
        
    }

    // 2. This represents an individual song
    public class SpotifyTrack
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("preview_url")]
        public string PreviewUrl { get; set; } = string.Empty;
        [JsonPropertyName("album")]
        public SpotifyAlbum? Album { get; set; }
    }

    // 3. This holds the albu, data attached to the song
    public class SpotifyAlbum
    {
        [JsonPropertyName("images")]
        public List<SpotifyImage> Images { get; set; } = new List<SpotifyImage>();
    }

    // 4. This holds the URL for the album cover art
    public class SpotifyImage
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}