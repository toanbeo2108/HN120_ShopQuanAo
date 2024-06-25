using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class ThanhToanRepository : IThanhToanRepository
    {
        private readonly AppDbContext _appDbContext;
        public ThanhToanRepository(AppDbContext context)
        {
                _appDbContext = context;
        }
        public void CreatThanhToan(ThanhToan tt)
        {
            try
            {
                var ma = _appDbContext.ThanhToan.FirstOrDefault(c=>c.MaPhuongThuc == tt.MaPhuongThuc);
                if (ma == null)
                {
                    var count = _appDbContext.ThanhToan.Count();
                    tt.MaPhuongThuc = "TT" + (count + 1);
                    _appDbContext.Add(tt);
                    _appDbContext.SaveChanges();
                }
                else
                {
                    throw new Exception( "đã tồn tại");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void DeleteThanhToan(string ma)
        {
            try
            {
                var matt = _appDbContext.ThanhToan.FirstOrDefault(c => c.MaPhuongThuc == ma);
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

        public IEnumerable<ThanhToan> GetAllThanhtoan()
        {
            return _appDbContext.ThanhToan;
        }

        public ThanhToan GetThanhToanByMa(string ma)
        {
            try
            {
                var matt = _appDbContext.ThanhToan.FirstOrDefault(c => c.MaPhuongThuc == ma);
                if (matt != null)
                {
                    return  matt;
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

        public void UpdateThanhToan(ThanhToan tt)
        {
            try
            {
                var thanhtoan = _appDbContext.ThanhToan.FirstOrDefault(c => c.MaPhuongThuc == tt.MaPhuongThuc);
                if (thanhtoan != null)
                {
                  //  thanhtoan.MaPhuongThuc = tt.MaPhuongThuc;
                    thanhtoan.TenPhuongThuc = tt.TenPhuongThuc;
                    thanhtoan.MoTa = tt.MoTa;
                    thanhtoan.NgayTao = tt.NgayTao;
                    thanhtoan.NgayThayDoi = tt.NgayThayDoi;
                    thanhtoan.TrangThai = tt.TrangThai;
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
