namespace HN120_ShopQuanAo.View.Areas.Admin.Data
{
    public class ThanhToan_ThanhToanHistory
    {
        // thanh toán
        public string MaPhuongThuc { get; set; }
        public string? TenPhuongThuc { get; set; }
        public string? MoTa { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayThayDoi { get; set; }      
        // thanh toán historry
        public string MaPhuongThuc_HoaDon { get; set; }
        public DateTime? NgayTaoThanhToan { get; set; }
        public DateTime? NgayThayDoiThanhToan { get; set; }
        // hóa đơn 
        public string MaHoaDon { get; set; }
        public string? UserID { get; set; }
        public string? MaVoucher { get; set; }
        public DateTime? NgayTaoDon { get; set; }
        public string? TenKhachHang { get; set; }
        public string? SoDienThoai { get; set; }
        public decimal? PhiShip { get; set; }
        public decimal? TongGiaTriHangHoa { get; set; }
        public int? PhuongThucThanhToan { get; set; }
        public int? TrangThai { get; set; }
    }
}
