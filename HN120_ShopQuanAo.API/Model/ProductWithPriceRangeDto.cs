namespace HN120_ShopQuanAo.API.Model
{
    public class ProductWithPriceRangeDto
    {
        public string MaSp { get; set; }
        public string? TenSP { get; set; }
        public string? UrlAvatar { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
