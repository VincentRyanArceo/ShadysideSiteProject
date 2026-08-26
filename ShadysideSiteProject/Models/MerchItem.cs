namespace ShadysideSiteProject.Models
{
    public class MerchItem
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsDigitalDownload { get; set; }

        // New property to hold the ZIP files
        public string DownloadUrl { get; set; } = string.Empty;
    }
}
