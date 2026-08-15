using Microsoft.AspNetCore.Mvc;
using ShadysideSiteProject.Services;
using ShadysideSiteProject.Models;

namespace ShadysideSiteProject.Controllers
{
    public class MusicController : Controller
    {
        private readonly SpotifyService _spotifyService;

        // Dependency Injection hands the controller the SpotifyService
        public MusicController(SpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        // The async method waits for the web responses
        public async Task<IActionResult> Index()
        {
            // 1. Call SpotifyServices to get the secure access token
            var token = await _spotifyService.GetAccessTokenAsync();

            // 2. Define the exact Artist ID we want to query (this is for the band "Shadyside")
            string artistId = "1lyXagmnPtEFA2PcjXDsLg";

            // 3. Fetch the top tracks using token
            var tracks = await _spotifyService.GetTopTracksAsync(token, artistId);

            // 4. Pass the list of the tracks directly to the View as its Model
            return View(tracks);
        }
    }
}
