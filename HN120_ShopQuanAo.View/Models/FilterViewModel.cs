using HN120_ShopQuanAo.API.Model;

namespace HN120_ShopQuanAo.View.Models
{
    public class FilterViewModel
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public List<ThuongHieuDto> ThuongHieus { get; set; } = new List<ThuongHieuDto>();
        public List<SizeDto> Sizes { get; set; } = new List<SizeDto>();
        public List<MauSacDto> MauSacs { get; set; } = new List<MauSacDto>();
        public List<ChatLieuDto> ChatLieus { get; set; } = new List<ChatLieuDto>();
        public List<TheLoaiDto> TheLoais { get; set; } = new List<TheLoaiDto>();
        public List<ProductWithPriceRangeDto> Products { get; set; } = new List<ProductWithPriceRangeDto>();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 9;
        public string? SortBy { get; set; }
    }

    public class ThuongHieuDto
    {
        public string MaThuongHieu { get; set; }
        public string TenThuongHieu { get; set; }
    }

    public class SizeDto
    {
        public string MaSize { get; set; }
        public string TenSize { get; set; }
    }

    public class MauSacDto
    {
        public string MaMau { get; set; }
        public string TenMau { get; set; }
    }

    public class ChatLieuDto
    {
        public string MaChatLieu { get; set; }
        public string TenChatLieu { get; set; }
    }

    public class TheLoaiDto
    {
        public string MaTheLoai { get; set; }
        public string TenTheLoai { get; set; }
    }


    public class MinMaxPriceDto
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
    public class ProductWithPriceRangeDto
    {
        public string MaSp { get; set; }
        public string? TenSP { get; set; }
        public string? UrlAvatar { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
