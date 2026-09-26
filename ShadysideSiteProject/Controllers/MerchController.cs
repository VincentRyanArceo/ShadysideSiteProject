using Microsoft.AspNetCore.Mvc;
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

        public MerchController()
        {
            _httpClient = new HttpClient();
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
                    ImageUrl = "/images/FlyOnGlass.jpg",
                    IsDigitalDownload = true,
                    PreviewAudioPath = "/audio/04 You Hate It.mp3",
                    Mp3DownloadUrl = "/downloads/_PlaceHolder(STREAM)_MP3.zip"
                },
                new MerchItem
                {
                    ID = 2,
                    Name = "Higher Plans EP",
                    Description = "Digital download of our Higher Plans EP.",
                    Price = 5.00m,
                    ImageUrl = "/images/_HigherPlansCover.jpg",
                    IsDigitalDownload = true,
                    PreviewAudioPath = "/audio/03 When In the Wars.mp3",
                    Mp3DownloadUrl = "/downloads/Shadyside_HigherPlans_MP3Edition.zip"
                }
            };

            // 2. Configure the Fourthwall API call
            string storefrontToken = ""; // Temporarily removed for GitHub push
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
                    else
                    {
                        ViewBag.ApiMessage = "API call succeeded, but 0 products were found. Ensure items are published in Fourthwall!";
                    }
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