namespace HN120_ShopQuanAo.View.Models
{
    public class FilterViewModel
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public List<ThuongHieuDto> ThuongHieus { get; set; }
        public List<SizeDto> Sizes { get; set; }
        public List<MauSacDto> MauSacs { get; set; }
        public List<ChatLieuDto> ChatLieus { get; set; }
        public List<TheLoaiDto> TheLoais { get; set; }
        public List<SanPhamDto> Products { get; set; }  // Danh sách sản phẩm sau khi lọc
        public int TotalItems { get; set; }  // Tổng số sản phẩm sau khi lọc
        public int PageNumber { get; set; }  // Trang hiện tại
        public int PageSize { get; set; }    // Số lượng sản phẩm trên mỗi trang
        public string? SortBy { get; set; }  // Tiêu chí sắp xếp
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
}
