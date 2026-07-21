using Microsoft.AspNetCore.Mvc;
using ShadysideSiteProject.Services;

namespace ShadysideSiteProject.Controllers
{
    public class MusicController : Controller
    {
        private readonly SpotifyService _spotifyService;

        // 1. Dependecy Injection: ASP.NET automatically hands the controller the SpotifyService
        public MusicController(SpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        // 2. Change this to async Task<IActionResult> b/c we are waiting for a web response
        public async Task<IActionResult> Index()
        {
            // 3. Call SpotifyService to get token
            var accessToken = await _spotifyService.GetAccessTokenAsync();

            // 4. Pass the access token to the view using ViewBag
            ViewBag.AccessToken = accessToken;

            return View();
        }
    }
}
