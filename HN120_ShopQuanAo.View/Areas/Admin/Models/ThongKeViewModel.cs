// File path: HN120_ShopQuanAo.View/Areas/Admin/Models/ThongKeViewModel.cs
using Newtonsoft.Json;

namespace HN120_ShopQuanAo.View.Areas.Admin.Models
{
    public class ThongKeViewModel
    {
        public decimal TongDoanhThu { get; set; }
        public int SoLuongSanPham { get; set; }
        public int SoLuongHoaDon { get; set; }
        public List<SanPhamBanChayViewModel> TopSanPhamBanChay { get; set; } = new List<SanPhamBanChayViewModel>();
        public List<SanPhamSapHetHangViewModel> SanPhamSapHetHang { get; set; } = new List<SanPhamSapHetHangViewModel>();

        // Thêm thuộc tính cho doanh thu theo ngày
        public List<DoanhThuViewModel> DoanhThuTheoNgay { get; set; } = new List<DoanhThuViewModel>();
    }

    public class SanPhamBanChayViewModel
    {
        public string SKU { get; set; }
        public string MaSp { get; set; } 
        public string TenSP { get; set; }
        public decimal? DonGia { get; set; }
        public int? SoLuongTon { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class SanPhamSapHetHangViewModel
    {
        public string SKU { get; set; }
        public string TenSP { get; set; }
        public int SoLuongTon { get; set; }
        public decimal GiaBan { get; set; }
    }

    public class DoanhThuViewModel
    {
        [JsonProperty("ngay")]
        public DateTime Ngay { get; set; }
        [JsonProperty("tongDoanhThu")]
        public decimal DoanhThu { get; set; }
    }
}
