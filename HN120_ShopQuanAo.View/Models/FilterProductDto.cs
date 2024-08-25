namespace HN120_ShopQuanAo.View.Models
{
    public class FilterProductDto
    {
        public string? MaThuongHieu { get; set; }
        public string? MaTheLoai { get; set; }
        public string? MaChatLieu { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? MaSize { get; set; }
        public string? MaMau { get; set; }
    }
}
