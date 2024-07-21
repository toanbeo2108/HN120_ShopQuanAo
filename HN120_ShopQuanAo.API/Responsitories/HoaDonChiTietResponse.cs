using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class HoaDonChiTietResponse : IHoaDonChiTietResponse
    {
        private readonly AppDbContext _context;
        public HoaDonChiTietResponse()
        {
            _context = new AppDbContext();
        }
        public async Task<bool> CreateHoaDonChiTiet(string? sku, string? maHD, string? tenSP, decimal? donGia, int? soluong)
        {
            try
            {
                HoaDonChiTiet hdct = new HoaDonChiTiet
                {
                    MaHoaDonChiTiet = Guid.NewGuid().ToString(),
                    SKU = sku,
                    MaHoaDon = maHD,
                    TenSp = tenSP,
                    DonGia = donGia,
                    SoLuongMua = soluong,
                    TrangThai = 1
                };
                await _context.HoaDonChiTiet.AddAsync(hdct);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
