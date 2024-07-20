using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface IHoaDonResponse
    {
        Task<bool> CreateHoaDonUser(string UserId);
        Task<IEnumerable<HoaDon>> GetAllHoaDonByUserId(string UserId);
        Task<IEnumerable<HoaDonChiTiet>> GetAllItemHoaDon(string MaHD);
        Task<bool> UpdateHoaDon(string maHD, string? MaVoucher, string? tenkh, string? sdt, decimal? phiship, decimal? tongtien, int? pttt, string? phanloai, string? ghichu,string? tinh, string? huyen, string? xa, string? cuthe);
    }
}
