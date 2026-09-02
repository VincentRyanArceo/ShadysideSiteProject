using Microsoft.AspNetCore.Mvc;
using ShadysideSiteProject.Models;

namespace ShadysideSiteProject.Controllers
{
    public class MerchController : Controller
    {
        public IActionResult Index()
        {
            var storeItems = new List<MerchItem>
            {
                new MerchItem
                {
                    ID = 1,
                    Name = "Higher Plans EP",
                    Description = "High-quality digital download of our latest release.",
                    Price = 5.00m,
                    ImageUrl = "/images/_Higher Plans Cover.jpg",
                    IsDigitalDownload = true,

                    // Pointing exactly to the first track in the audio folder
                    PreviewAudioPath = "/audio/01 When the Fear Hits.mp3",
                    //Pointing to the MP3 zip file
                    DownloadUrl = "/downloads/Shadyside_HigherPlans(MP3Edition)"
                }
            };
            return View(storeItems);
        }
    }
}
