using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class ThanhToanHoaDonRepository : IThanhToanHoaDonRepository
    {
        private readonly AppDbContext _appDbContext;
        public ThanhToanHoaDonRepository(AppDbContext appDb)
        {
            _appDbContext = appDb;
        }
        public void CreateThanhToan_HoaDon(ThanhToan_HoaDon tt)
        {
            try
            {
                var ma = _appDbContext.ThanhToan_HoaDon.FirstOrDefault(c => c.MaPhuongThuc_HoaDon == tt.MaPhuongThuc_HoaDon);
                if (ma == null)
                {
                    var count = _appDbContext.ThanhToan_HoaDon.Count();
                    tt.MaPhuongThuc_HoaDon = "TTHD" + (count + 1);
                    _appDbContext.Add(tt);
                    _appDbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("đã tồn tại");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void DeleteThanhToan_HoaDon(string ma)
        {
            try
            {
                var matt = _appDbContext.ThanhToan_HoaDon.FirstOrDefault(c => c.MaPhuongThuc_HoaDon == ma);
                if (matt != null)
                {
                    _appDbContext.Remove(matt);
                    _appDbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("không tồn tại");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<ThanhToan_HoaDon> GetAllThanhToan_HoaDon()
        {
            return _appDbContext.ThanhToan_HoaDon;
        }

        public ThanhToan_HoaDon GetThanhToan_HoaDonByMa(string ma)
        {
            try
            {
                var matt = _appDbContext.ThanhToan_HoaDon.FirstOrDefault(c => c.MaPhuongThuc_HoaDon == ma);
                if (matt != null)
                {
                    return matt;
                }
                else
                {
                    throw new Exception("không tồn tại");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void UpdateThanhToan_HoaDon(ThanhToan_HoaDon tt)
        {
            try
            {
                var thanhtoanhd = _appDbContext.ThanhToan_HoaDon.FirstOrDefault(c => c.MaPhuongThuc_HoaDon == tt.MaPhuongThuc_HoaDon);
                if (thanhtoanhd != null)
                {
                  //  thanhtoanhd.MaPhuongThuc_HoaDon = tt.MaPhuongThuc_HoaDon;
                    thanhtoanhd.MaPhuongThuc = tt.MaPhuongThuc;
                    thanhtoanhd.MaHoaDon = tt.MaHoaDon;
                    thanhtoanhd.MoTa = tt.MoTa;
                    thanhtoanhd.NgayThayDoi = tt.NgayThayDoi;
                    thanhtoanhd.NgayTao = tt.NgayTao;
                    thanhtoanhd.TrangThai = tt.TrangThai;
                    _appDbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("không tồn tại");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
