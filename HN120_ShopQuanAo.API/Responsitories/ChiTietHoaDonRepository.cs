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
        public void CreateCTHD(List<HoaDonChiTiet> hdct)
        {
            DateTime now = DateTime.Now;
            string formattedTime = now.ToString("yyyyMMddHHmmss");
            var totalHoaDonCT = _appDbContext.HoaDonChiTiet.Count();
            foreach (var item in hdct)
            {
                item.MaHoaDonChiTiet = ("HDCT" + formattedTime + (totalHoaDonCT +1)).ToString();
                totalHoaDonCT++;
            }
            var hoaDonExists = _appDbContext.HoaDon.FirstOrDefault(c => c.MaHoaDon == hdct[0].MaHoaDon);
            if (hoaDonExists == null)
            {
                throw new Exception("Mã hóa đơn không tồn tại");
            }

            foreach (var hd in hdct)
            {
                var ctsanpham = _appDbContext.ChiTietSp.FirstOrDefault(c => c.SKU == hd.SKU);
                var sanpham = _appDbContext.SanPham.FirstOrDefault(c => c.MaSp == ctsanpham.MaSp);
                var hoadon = _appDbContext.HoaDon.FirstOrDefault(c => c.MaHoaDon == hd.MaHoaDon);
                if (ctsanpham != null)
                {
                    //if (ctsanpham.SoLuongTon < hd.SoLuongMua)
                    //{
                    //    throw new Exception("Số lượng sản phẩm còn lại khồn đủ đáp ứng");
                    //}
                    //else if (ctsanpham.SoLuongTon <= 0)
                    //{
                    //    throw new Exception("Sản phẩm này đã hết");
                    //}
                    //else
                    //{
                        _appDbContext.HoaDonChiTiet.Add(hd);
                   // }
                }
              
            }
            _appDbContext.SaveChanges();
        }

        public void DeleteCTHD(string ma)
        {
            var hdct = _appDbContext.HoaDonChiTiet.FirstOrDefault(c => c.MaHoaDonChiTiet == ma);
            if (hdct != null)
            {
                var ctsp = _appDbContext.ChiTietSp.FirstOrDefault(c => c.SKU == hdct.SKU);
                var hoadon = _appDbContext.HoaDon.FirstOrDefault(c => c.MaHoaDon == hdct.MaHoaDon);
                if (ctsp != null)
                {
                    if (hoadon.TrangThai > 1) { 
                    ctsp.SoLuongTon += hdct.SoLuongMua;
                    }
                }
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

        public void UpdateCTHD(List<HoaDonChiTiet> hdctList)
        {
            foreach(var hdct in hdctList)
            {
               
                var mshd = _appDbContext.HoaDonChiTiet.FirstOrDefault(c => c.MaHoaDonChiTiet == hdct.MaHoaDonChiTiet);
                if (mshd == null)
                {
                    throw new Exception($"Mã hóa đơn chi tiết {hdct.MaHoaDonChiTiet} không tồn tại");

                }
                var hoaDonExists = _appDbContext.HoaDon.FirstOrDefault(c => c.MaHoaDon == hdct.MaHoaDon);
                if (hoaDonExists == null)
                {
                    throw new Exception($"Mã hóa đơn {hdct.MaHoaDon} không tồn tại");
                }
                var sanphamchitiet = _appDbContext.ChiTietSp.FirstOrDefault(c=>c.SKU == hdct.SKU);
                if (sanphamchitiet == null)
                {
                    throw new Exception($"SKU {hdct.SKU} không tồn tại");
                }
                
                sanphamchitiet.SoLuongTon -= hdct.SoLuongMua;
                
                mshd.SKU = hdct.SKU;
                mshd.MaHoaDon = hdct.MaHoaDon;
                mshd.TenSp = hdct.TenSp;
                mshd.DonGia = hdct.DonGia;
                mshd.SoLuongMua = hdct.SoLuongMua;
                mshd.TrangThai = hdct.TrangThai;
            }
            _appDbContext.SaveChanges();
        }
    }
}
