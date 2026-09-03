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
                    ImageUrl = "/images/_HigherPlansCover.jpg",
                    IsDigitalDownload = true,

                    // Pointing exactly to the first track in the audio folder
                    PreviewAudioPath = "/audio/03 When In The Wars.mp3",
                    //Pointing to the MP3 zip file
                    Mp3DownloadUrl = "/downloads/Shadyside_HigherPlans(MP3Edition).zip",
                    WavDownloadUrl = "/downloads/Shadyside_HigherPlans(WAVEdition).zip"
                }
            };
            return View(storeItems);
        }
    }
}
