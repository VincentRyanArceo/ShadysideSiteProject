using System.Text.Json.Serialization;

    namespace ShadysideSiteProject.Models
{
    // 1. The top-level response holding the list of products
    public class FourthwallProductResponse
    {
        [JsonPropertyName("results")]
        public List<FourthwallProduct> Results { get; set; } = new List<FourthwallProduct>();
    }

    // 2. The core details for each individual physical item
    public class FourthwallProduct
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("images")]
        public List<FourthwallImage> Images { get; set; } = new List<FourthwallImage>();
        [JsonPropertyName("variants")]
        public List<FourthwallVariant> Variants { get; set; } = new List<FourthwallVariant>();
    }

    // 3. Extracts the product image URL from Fourthwalls nested image list
    public class FourthwallImage
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    // 4. Extracts the price tier (variants cover different sizes/colors, etc.)
    public class FourthwallVariant
    {
        [JsonPropertyName("unit_price")]
        public FourthwallPrice UnitPrice { get; set; } = new FourthwallPrice();
    }

    // 5. Isolates the actual decimal value of the price
    public class FourthwallPrice
    {
        [JsonPropertyName("value")]
        public string Value { get; set; } = "0.00";
    }
}
