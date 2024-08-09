using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Configurations;
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
                    NgayThayDoi = DateTime.Now,
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

        public async Task<bool> CreateUVC(string userid, string mavc)
        {
            User_Voucher UVC = new User_Voucher
            {
                UserVoucherID = Guid.NewGuid().ToString(),
                UserID = userid,
                MaVoucher = mavc,
                TrangThai = 1
            };
            await _context.User_Voucher.AddAsync(UVC);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<HoaDon>> GetAllHoaDonByUserId(string UserId)
        {
            return await _context.HoaDon.Where(x => x.UserID == UserId).ToListAsync();

        }

        public async Task<IEnumerable<HoaDonChiTiet>> GetAllItemHoaDon(string MaHD)
        {
            return await _context.HoaDonChiTiet.Where(x => x.MaHoaDon == MaHD).ToListAsync();
        }

        public async Task<IEnumerable<User_Voucher>> GetVoucherbyUserid(string userid)
        {
            return await _context.User_Voucher.Where(x => x.UserID == userid).ToListAsync();
        }

        public async Task<bool> HuyDon(string maHD)
        {
            try
            {
                var hd = await _context.HoaDon.FirstOrDefaultAsync(x => x.MaHoaDon == maHD);
                if (hd == null)
                {
                    return false;
                }
                hd.UserID = hd.UserID;
                hd.MaVoucher = hd.MaVoucher;
                hd.NgayTaoDon = hd.NgayTaoDon;
                hd.NgayThayDoi = hd.NgayThayDoi;
                hd.TenKhachHang = hd.TenKhachHang;
                hd.SoDienThoai = hd.SoDienThoai;
                hd.PhiShip = hd.PhiShip;
                hd.TongGiaTriHangHoa = hd.TongGiaTriHangHoa;
                hd.PhuongThucThanhToan = hd.PhuongThucThanhToan;
                hd.PhanLoai = hd.PhanLoai;
                hd.Ghichu = hd.Ghichu;
                hd.TinhThanh = hd.TinhThanh;
                hd.QuanHuyen = hd.QuanHuyen;
                hd.XaPhuong = hd.XaPhuong;
                hd.Cuthe = hd.Cuthe;
                hd.TrangThai = 6;

                _context.HoaDon.Update(hd);
                await _context.SaveChangesAsync();
                
                HoaDon_History hdhs = new HoaDon_History
                {
                    LichSuHoaDonID = DateTime.Now.ToString(),
                    MaHoaDon = hd.MaHoaDon,
                    UserID = hd.UserID,
                    NgayTaoDon = DateTime.Now,
                    NgayThayDoi = DateTime.Now,
                    TongGiaTri = hd.TongGiaTriHangHoa,
                    HinhThucThanhToan = "4",
                    ChiTiet = "Tôi Không còn nhu cầu mua hàng",
                    TrangThai = 6
                };
                _context.HoaDon_History.Add(hdhs);
                await _context.SaveChangesAsync();

                ThanhToan_HoaDon tthd = new ThanhToan_HoaDon
                {
                    MaPhuongThuc_HoaDon = hd.MaHoaDon + "_4",
                    MaHoaDon = hd.MaHoaDon,
                    MaPhuongThuc = "4",
                    MoTa = "",
                    NgayTao = DateTime.Now,
                    NgayThayDoi = DateTime.Now,
                    TrangThai = 6
                };
                _context.ThanhToan_HoaDon.Add(tthd);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
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
                hd.NgayThayDoi = DateTime.Now;
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

                HoaDon_History hdhs = new HoaDon_History
                {
                    LichSuHoaDonID = DateTime.Now.ToString(),
                    MaHoaDon = hd.MaHoaDon,
                    UserID = hd.UserID,
                    NgayTaoDon = DateTime.Now,
                    NgayThayDoi = DateTime.Now,
                    TongGiaTri = hd.TongGiaTriHangHoa,
                    HinhThucThanhToan = "4",
                    ChiTiet = "",
                    TrangThai = 0
                };
                _context.HoaDon_History.Add(hdhs);
                await _context.SaveChangesAsync();

                ThanhToan_HoaDon tthd = new ThanhToan_HoaDon
                {
                    MaPhuongThuc_HoaDon = hd.MaHoaDon + "_4",
                    MaHoaDon = hd.MaHoaDon,
                    MaPhuongThuc = "4",
                    MoTa = "",
                    NgayTao = DateTime.Now,
                    NgayThayDoi = DateTime.Now,
                    TrangThai = 0
                };
                _context.ThanhToan_HoaDon.Add(tthd);
                await _context.SaveChangesAsync();  
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> UpdateUser_Voucher(string user_voucherid)
        {
            var uvc =  _context.User_Voucher.FirstOrDefault(x => x.UserVoucherID ==  user_voucherid);
            if(uvc != null)
            {
                uvc.UserID = uvc.UserID;
                uvc.MaVoucher = uvc.MaVoucher;
                uvc.TrangThai = 0;
                _context.User_Voucher.Update(uvc);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
