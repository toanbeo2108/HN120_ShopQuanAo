using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface IHoaDonChiTietResponse
    {
        //public string MaHoaDonChiTiet { get; set; }
        //public string? SKU { get; set; }
        //public string? MaHoaDon { get; set; }
        //public string? TenSp { get; set; }
        //public decimal? DonGia { get; set; }
        //public int? SoLuongMua { get; set; }
        //public int? TrangThai { get; set; }

        public Task<bool> CreateHoaDonChiTiet(string? sku, string? maHD, string? tenSP, decimal? donGia, int? soluong);
        public Task<bool> CreateHoaDonChiTiet2(HoaDonChiTiet? hdct);
    }
}
