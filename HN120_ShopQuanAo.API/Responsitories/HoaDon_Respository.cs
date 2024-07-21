using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class HoaDon_Respository : IHoaDon_Respository
    {
        private readonly AppDbContext _context;
        public HoaDon_Respository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateHoaDon(HoaDon hoaDon)
        {
            var hd = _context.HoaDon.FirstOrDefault(c => c.MaHoaDon == hoaDon.MaHoaDon);
            if (hd != null)
            {
                throw new Exception("Hóa đơn đã tồn tại");
            }          
            _context.HoaDon.Add(hoaDon);
            _context.SaveChanges();
        }

        public void DeleteHoaDon(string ma)
        {
            var mahd = _context.HoaDon.FirstOrDefault(c => c.MaHoaDon == ma);

            if (mahd != null)
            {
                var list = _context.HoaDonChiTiet.Where(c => c.MaHoaDon == ma).ToList();
                foreach (var ct in list)
                {
                    _context.HoaDonChiTiet.Remove(ct);
                }
                _context.HoaDon.Remove(mahd);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Hóa đơn không tồn tại.");
            }
        }

        public IEnumerable<HoaDon> GetAllHoaDon()
        {
            return _context.HoaDon;
        }

        public HoaDon GetHoaDonByMa(string ma)
        {
            var mahd = _context.HoaDon.FirstOrDefault(c => c.MaHoaDon == ma);
            if (mahd != null)
            {
                return mahd;
            }
            else
            {
                throw new Exception("Hóa đơn không tồn tại.");
            }
        }

        public IEnumerable<HoaDon> GetHoaDonByTrangthai(int stt)
        {
            return _context.HoaDon.Where(hd => hd.TrangThai == stt).ToList();
        }

        public IEnumerable<dynamic> GetHoaDonWithDetails(string ma)
        {
            var result = from hd in _context.HoaDon
                         where hd.MaHoaDon == ma
                         select new
                         {
                             HoaDon = hd,
                             HoaDonChiTiets = _context.HoaDonChiTiet.Where(hct => hct.MaHoaDon == ma).ToList()
                         };

            return result.ToList();
        }

        public void UpdateHoaDon(HoaDon hoaDon)
        {
            var update = _context.HoaDon.FirstOrDefault(c => c.MaHoaDon == hoaDon.MaHoaDon);
            var voucher = _context.Voucher.FirstOrDefault(c=>c.MaVoucher == hoaDon.MaVoucher);
            if (update == null)
            {
                throw new Exception("Hóa đơn không tồn tại.");
            }
         

            update.UserID = hoaDon.UserID;
            update.MaVoucher = hoaDon.MaVoucher;
            update.NgayTaoDon = hoaDon.NgayTaoDon;
            update.NgayThayDoi = hoaDon.NgayThayDoi;
            update.TenKhachHang = hoaDon.TenKhachHang;
            update.SoDienThoai = hoaDon.SoDienThoai;
            update.PhiShip = hoaDon.PhiShip;
            update.TongGiaTriHangHoa = hoaDon.TongGiaTriHangHoa;
            update.PhuongThucThanhToan = hoaDon.PhuongThucThanhToan;
            update.TrangThai = hoaDon.TrangThai;
            update.PhanLoai = hoaDon.PhanLoai;
            update.TinhThanh = hoaDon.TinhThanh;
            update.QuanHuyen = hoaDon.QuanHuyen;
            update.XaPhuong = hoaDon.XaPhuong;
            update.Cuthe = hoaDon.Cuthe;
            update.Ghichu = hoaDon.Ghichu;
            _context.SaveChanges();
        }
    }
}
