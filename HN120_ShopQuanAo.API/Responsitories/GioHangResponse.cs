using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class GioHangResponse : IGioHangReponsitory
    {
        private readonly AppDbContext _appDbContext;
        public GioHangResponse()
        {
            _appDbContext = new AppDbContext();
        }
        public Task<bool> CreateGioHang(string MaGH, string? UserID, decimal? TongTien, string? mota, int? trangthai)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteGioHang(string MaGH)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GioHang>> GetAllGioHang()
        {
            throw new NotImplementedException();
        }

        public Task<List<GioHang>> GetAllHangById(string UserId, int? Status)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateGioHang(string MaGH, decimal? TongTien, int? trangthai)
        {
            var gh =  _appDbContext.GioHang.FirstOrDefault(x => x.MaGioHang == MaGH);
            if(gh == null)
            {
                return false;
            }
            gh.TongTien = TongTien;
            gh.TrangThai = trangthai;
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
