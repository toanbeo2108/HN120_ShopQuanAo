namespace HN120_ShopQuanAo.View.Areas.Admin.Data
{
    public class ChiTietSPView
    {
        //Sản phẩm
        public string MaSp { get; set; }
        public string? MaThuongHieu { get; set; }
        public string? MaTheLoai { get; set; }
        public string? UrlAvatar { get; set; }
        public string? TenSP { get; set; }
        public string? Mota { get; set; }
        public int? TongSoLuong { get; set; }
        //chi tiết sản phẩm
        public string SKU { get; set; }
        public string? MaSize { get; set; }
        public string? MaMau { get; set; }
        public string? MaKhuyenMai { get; set; }
        public string? MaChatLieu { get; set; }
        public string? UrlAnhSpct { get; set; }
        public decimal? Dongia { get; set; }
        public decimal? GiaBan { get; set; }
        public int? SoLuongTon { get; set; }
        public int? TrangThai { get; set; }     
        public string? TenMau { get; set; }
        public string? TenSize { get; set; }
        //phương thức thanh toán
        //public string MaPhuongThuc { get; set; }
        //public string? TenPhuongThuc { get; set; }
        //public string? MoTa { get; set; }
        //public DateTime? NgayTao { get; set; }
        //public DateTime? NgayThayDoi { get; set; }
        //thanh toán hóa đơn
        //public string MaPhuongThuc_HoaDon { get; set; }
        //public string? MaHoaDon { get; set; }
        //public DateTime? NgayTaoThanhToan { get; set; }
        //public DateTime? NgayThayDoiThanhtoan { get; set; }
        //public int? TrangThaithanhtoan { get; set; }
    }
}
