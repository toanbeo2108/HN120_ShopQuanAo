using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.IResponsitories
{
    public interface IGioHangReponsitory
    {
        Task<IEnumerable<GioHang>> GetAllGioHang();
        Task<List<GioHang>> GetAllHangById(string UserId, int? Status);
        Task<bool> CreateGioHang(string MaGH, string? UserID, decimal? TongTien,  string? mota, int? trangthai);
        Task<bool> UpdateGioHang(string MaGH, string? UserID, decimal? TongTien, string? mota, int? trangthai);
        Task<bool> DeleteGioHang(string MaGH);
    }
}
