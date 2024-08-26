namespace HN120_ShopQuanAo.View.Areas.Customer.Data
{
    public class HDCTViewModels
    {
        public string maHDCT { get; set; }
        public string MaSp { get; set; }
        public string? MaThuongHieu { get; set; }
        public string? MaTheLoai { get; set; }
        public string? UrlAvatar { get; set; }
        public string? TenSP { get; set; }
        public int? SoLuong { get; set; }
        public decimal? Dongia { get; set; }
        public decimal? ThanhTien { get; set; }

        //chi tiết sản phẩm
        public string SKU { get; set; }
        public string? MaSize { get; set; }
        public string? MaMau { get; set; }
        public string? MaKhuyenMai { get; set; }
        public string? MaChatLieu { get; set; }
        public int? TrangThai { get; set; }
        public string? TenMau { get; set; }
        public string? TenSize { get; set; }
    }
}
