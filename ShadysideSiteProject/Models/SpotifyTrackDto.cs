using ShadysideSiteProject.Services;
using System.Text.Json.Serialization;

namespace ShadysideSiteProject.Models
{
    // 1. The outermost response from the Search API
    public class SpotifySearchResponse
    {
        [JsonPropertyName("tracks")]
        public SpotifyTracksContainer? Tracks { get; set; }
    }

    // 2. The container that holds the list of items
    public class SpotifyTracksContainer
    {
        [JsonPropertyName("items")]
        public List<SpotifyTrack> Items { get; set; } = new List<SpotifyTrack>();
    }

    // 3. This represents an individual song
    public class SpotifyTrack
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("preview_url")]
        public string PreviewUrl { get; set; } = string.Empty;
        [JsonPropertyName("album")]
        public SpotifyAlbum? Album { get; set; }
    }

    // 4. This holds the album data attached to the song
    public class SpotifyAlbum
    {
        [JsonPropertyName("images")]
        public List<SpotifyImage> Images { get; set; } = new List<SpotifyImage>();
    }

    // 5. This holds the URL for the album cover art
    public class SpotifyImage
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}