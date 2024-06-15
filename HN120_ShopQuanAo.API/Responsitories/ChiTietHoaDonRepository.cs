using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class ChiTietHoaDonRepository : IChiTietHoaDonRepository
    {
        private readonly AppDbContext _appDbContext;
        public ChiTietHoaDonRepository(AppDbContext db)
        {
            _appDbContext = db;
        }
        public void CreateCTHD(HoaDonChiTiet hdct)
        {
            var totalHoaDonCT = _appDbContext.HoaDonChiTiet.Count();
            hdct.MaHoaDonChiTiet = "HDCT" + (totalHoaDonCT + 1);
            var hoaDonExists = _appDbContext.HoaDon.FirstOrDefault(c => c.MaHoaDon == hdct.MaHoaDon);
            if (hoaDonExists == null)
            {
                throw new Exception("Mã hóa đơn không tồn tại");
            }
            _appDbContext.HoaDonChiTiet.Add(hdct);
            _appDbContext.SaveChanges();
        }

        public void DeleteCTHD(string ma)
        {
            var hdct = _appDbContext.HoaDonChiTiet.FirstOrDefault(c => c.MaHoaDonChiTiet == ma);
            if (hdct != null)
            {
                _appDbContext.Remove(hdct);
                _appDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Không tìm thấy hóa đơn chi tiết ");
            }
        }

        public IEnumerable<HoaDonChiTiet> GetAllHoaDonChiTiet()
        {
            return _appDbContext.HoaDonChiTiet;
        }

        public HoaDonChiTiet GetHoaDonChiTietByMa(string ma)
        {
            var mshd = _appDbContext.HoaDonChiTiet.FirstOrDefault(c => c.MaHoaDonChiTiet == ma);
            if (mshd != null)
            {
                return mshd;
            }
            else
            {
                throw new Exception("Mã hóa đơn chi tiết không tồn tại");
            }
        }

        public void UpdateCTHD(HoaDonChiTiet hdct)
        {
            var mshd = _appDbContext.HoaDonChiTiet.FirstOrDefault(c => c.MaHoaDonChiTiet == hdct.MaHoaDonChiTiet);
            if (mshd == null)
            {
                throw new Exception("Mã hóa đơn chi tiết không tồn tại");

            }
            var hoaDonExists = _appDbContext.HoaDon.FirstOrDefault(c => c.MaHoaDon == hdct.MaHoaDon);
            if (hoaDonExists == null)
            {
                throw new Exception("Mã hóa đơn không tồn tại");
            }
            mshd.SKU = hdct.SKU;
            mshd.MaHoaDon = hdct.MaHoaDon;
            mshd.TenSp = hdct.TenSp;
            mshd.DonGia = hdct.DonGia;
            mshd.SoLuongMua = hdct.SoLuongMua;
            mshd.TrangThai = hdct.TrangThai;
            _appDbContext.SaveChanges();
        }
    }
}
