using Microsoft.AspNetCore.Mvc;

namespace ShadysideSiteProject.Controllers
{
    public class MusicController : Controller
    {
        public IActionResult Index()
        {
            // We load the view, and the HTML iframe will securely stream the music.
            return View();
        }
    }
}