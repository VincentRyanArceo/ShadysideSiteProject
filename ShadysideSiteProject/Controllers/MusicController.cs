using Microsoft.AspNetCore.Mvc;

namespace ShadysideSiteProject.Controllers
{
    public class MusicController : Controller
    {
        // The Index action is the default method that runs when the cotroller is called
        public IActionResult Index()
        {
            // This tells the app to find and display the HTML view
            return View();
        }
    }
}
