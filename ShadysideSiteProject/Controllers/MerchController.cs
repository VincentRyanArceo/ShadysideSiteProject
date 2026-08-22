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
                    Name = "Higher Plans EP (Digital Download",
                    Description = "High-quality digital download of our latest release.",
                    Price = 5.00m,
                    ImageUrl = "/images/_Higher Plans Cover.jpg",
                    IsDigitalDownload = true
                }
            };
            return View();
        }
    }
}
