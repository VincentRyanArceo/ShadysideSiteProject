using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShadysideSiteProject.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace ShadysideSiteProject.Controllers
{
    public class MerchController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        // IConfiguration is injected here to read your secrets.json file
        public MerchController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Initialize the existing digital audio items
            var storeItems = new List<MerchItem>
    {
        new MerchItem
        {
            ID = 1,
            Name = "_PlaceHolder",
            Description = "Digital download of our latest release.",
            Price = 12.50m,
            ImageUrl = "/images/FlyOnGlass.jpeg",
            IsDigitalDownload = true,
            IsNewRelease = true,
            PreviewAudioPath = "/audio/03 The End of Everything.mp3",
            Mp3DownloadUrl = "/downloads/_PlaceHolder.zip"
        },
        new MerchItem
        {
            ID = 2,
            Name = "Higher Plans",
            Description = "Digital download of our Higher Plans EP.",
            Price = 5.00m,
            ImageUrl = "/images/_HigherPlansCover.jpg",
            IsDigitalDownload = true,
            IsNewRelease = true,
            PreviewAudioPath = "/audio/03 When In the Wars.mp3",
            Mp3DownloadUrl = "/downloads/Shadyside_HigherPlans_MP3Edition.zip"
        },
        new MerchItem
        {
            ID = 3,
            Name = "Later In The Past",
            Description = "Classic digital download.",
            Price = 5.00m,
            ImageUrl = "/images/LaterInThePast_CoverImage.jpg",
            IsDigitalDownload = true,
            IsNewRelease = false, // Flagged as Classic
            PreviewAudioPath = "/audio/02 as two fallen stars.mp3",
            Mp3DownloadUrl = "/downloads/LaterInThePast.zip"
        },
        new MerchItem
        {
            ID = 4,
            Name = "thefairbornohioband",
            Description = "Early years of Shadyside",
            Price = 3.00m,
            ImageUrl = "/images/TheFairibornOhioBand_Image.jpg",
            IsDigitalDownload = true,
            IsNewRelease = false,
            PreviewAudioPath = "/audio/05 glass hearts tend to break.mp3",
            Mp3DownloadUrl = "/downloads/TheFairbornOhioBand.zip"
        },
        new MerchItem
        {
            ID = 5,
            Name = "The Analog Sessions",
            Description = "Unreleased tracks",
            Price = 3.00m,
            ImageUrl = "/images/TheAnalogSessions_CoverImage.jpg",
            IsDigitalDownload = true,
            IsNewRelease = false,
            PreviewAudioPath = "/audio/01 ambulance.mp3",
            Mp3DownloadUrl = "/downloads/TheAnalogSessions.zip"
        },
        new MerchItem
        {
            ID = 6,
            Name = "...59",
            Description = "The Fianl EP Demos",
            Price = 3.00m,
            ImageUrl = "/images/_59_CoverImage.jpg",
            IsDigitalDownload = true,
            IsNewRelease = false,
            PreviewAudioPath = "/audio/02 cest le vin, stupide.mp3",
            Mp3DownloadUrl = "/downloads/_59.zip"
        }
    };

            // 2. Read token securely and validate
            string storefrontToken = _configuration["FourthwallApi:Token"];

            if (string.IsNullOrEmpty(storefrontToken))
            {
                ViewBag.ApiMessage = "Local Secret Missing: The API token was not found in User Secrets.";
                return View(storeItems);
            }

            string apiUrl = $"https://storefront-api.fourthwall.com/v1/products?storefront_token={storefrontToken}";

            try
            {
                // Send the request to Fourthwall
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    var fourthwallData = JsonSerializer.Deserialize<FourthwallProductResponse>(jsonResult);

                    if (fourthwallData != null && fourthwallData.Results != null && fourthwallData.Results.Count > 0)
                    {
                        // 3. Map Fourthwall data into our MerchItem list
                        foreach (var fwProduct in fourthwallData.Results)
                        {
                            decimal itemPrice = 0.00m;
                            if (fwProduct.Variants.Count > 0)
                            {
                                decimal.TryParse(fwProduct.Variants[0].UnitPrice.Value, out itemPrice);
                            }

                            string imageUrl = fwProduct.Images.Count > 0 ? fwProduct.Images[0].Url : "/images/skull crosshairs.png";

                            storeItems.Add(new MerchItem
                            {
                                ID = storeItems.Count + 1,
                                Name = fwProduct.Name,
                                Description = fwProduct.Description,
                                Price = itemPrice,
                                ImageUrl = imageUrl,
                                IsDigitalDownload = false // Pushes to the physical tier
                            });
                        }
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Gracefully handle the 404 empty catalog scenario
                    ViewBag.ApiMessage = "The Fourthwall store is connected, but no products are published yet!";
                }
                else
                {
                    ViewBag.ApiMessage = $"API failed with status code: {response.StatusCode}. Double check your token.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ApiMessage = $"Code Error: {ex.Message}";
            }

            return View(storeItems);
        }
    }
}