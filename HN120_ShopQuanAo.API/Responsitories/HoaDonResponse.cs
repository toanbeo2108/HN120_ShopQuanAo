using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class HoaDonResponse : IHoaDonResponse
    {
        private readonly AppDbContext _context;
        public HoaDonResponse()
        {
            _context = new AppDbContext();
        }
        public async Task<bool> CreateHoaDonUser(string UserId)
        {
            try
            {
                HoaDon hd = new HoaDon
                {
                    MaHoaDon = Guid.NewGuid().ToString(),
                    UserID = UserId,
                    MaVoucher = null,
                    NgayTaoDon = DateTime.Now,
                    NgayThayDoi = null,
                    TenKhachHang = null,
                    SoDienThoai = null,
                    PhiShip = null,
                    TongGiaTriHangHoa = null,
                    PhuongThucThanhToan = null,
                    PhanLoai = "2",
                    Ghichu = null,
                    TinhThanh = null,
                    QuanHuyen = null,
                    XaPhuong = null,
                    Cuthe = null,
                    TrangThai = 0

                };
                await _context.HoaDon.AddAsync(hd);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<IEnumerable<HoaDon>> GetAllHoaDonByUserId(string UserId)
        {
            return await _context.HoaDon.Where(x => x.UserID == UserId).ToListAsync();

        }

        public async Task<IEnumerable<HoaDonChiTiet>> GetAllItemHoaDon(string MaHD)
        {
            return await _context.HoaDonChiTiet.Where(x => x.MaHoaDon == MaHD).ToListAsync();
        }

        public async Task<bool> UpdateHoaDon(string maHD, string? MaVoucher, string? tenkh, string? sdt, decimal? phiship, decimal? tongtien, int? pttt, string? phanloai, string? ghichu, string? tinh, string? huyen, string? xa, string? cuthe)
        {
            try
            {
                var hd = await _context.HoaDon.FirstOrDefaultAsync(x => x.MaHoaDon == maHD);
                if (hd == null)
                {
                    return false;
                }
                hd.UserID = hd.UserID;
                hd.MaVoucher = MaVoucher;
                hd.NgayTaoDon = DateTime.Now;
                hd.NgayThayDoi = null;
                hd.TenKhachHang = tenkh;
                hd.SoDienThoai = sdt;
                hd.PhiShip = phiship;
                hd.TongGiaTriHangHoa = tongtien;
                hd.PhuongThucThanhToan = pttt;
                hd.PhanLoai = phanloai;
                hd.Ghichu = ghichu;
                hd.TinhThanh = tinh;
                hd.QuanHuyen = huyen;
                hd.XaPhuong = xa;
                hd.Cuthe = cuthe;
                hd.TrangThai = 1;
                _context.HoaDon.Update(hd);
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
