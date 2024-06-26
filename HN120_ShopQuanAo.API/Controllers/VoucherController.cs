using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HN120_ShopQuanAo.API.Controllers
{

    public class VoucherController : ControllerBase
    {
        private readonly IAllResponsitories<Voucher> _irespon;
        AppDbContext _context = new AppDbContext();
        public VoucherController()
        {
            _irespon = new AllResponsitories<Voucher>(_context, _context.Voucher);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Voucher>> GetAllVoucher()
        {
            return await _irespon.GetAll();
        }

        [HttpGet("[Action]/{id}")]
        public async Task<Voucher> GetCLById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateVCher(string? MaVoucher, string? Ten, int? KieuGiamGia, decimal? GiaGiamToiThieu, decimal? GiaGiamToiDa, DateTime? NgayBatDau, DateTime? NgayKetThuc, decimal? GiaTriGiam, int? SoLuong, string? MoTa, int? TrangThai)
        {
            var lstVC = await _irespon.GetAll();
            var cl = lstVC.FirstOrDefault(x => x.MaVoucher == MaVoucher);

            int clCount = lstVC.Count() + 1;
            if (cl != null)
            {
                return false;
            }
            Voucher b = new Voucher();
            b.MaVoucher = "VC" + clCount.ToString();
            b.Ten = Ten;
            b.KieuGiamGia = KieuGiamGia;
            b.GiaGiamToiThieu = GiaGiamToiThieu;
            b.GiaGiamToiDa = GiaGiamToiDa;
            b.NgayBatDau = NgayBatDau;
            b.NgayKetThuc = NgayKetThuc;
            b.GiaTriGiam = GiaTriGiam;
            b.SoLuong = SoLuong;
            b.MoTa = MoTa;
            b.TrangThai = TrangThai;

            return await _irespon.CreateItem(b);
        }


        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateVCher(string id, [FromBody] Voucher _ctsp)
        {
            {
                var ctsp = await _irespon.GetAll();
                var b = ctsp.FirstOrDefault(c => c.MaVoucher == id);
                if (b == null)
                {
                    return false;
                }
                else
                {
                    b.Ten = _ctsp.Ten;
                    b.KieuGiamGia = _ctsp.KieuGiamGia;
                    b.GiaGiamToiThieu = _ctsp.GiaGiamToiThieu;
                    b.GiaGiamToiDa = _ctsp.GiaGiamToiDa;
                    b.NgayBatDau = _ctsp.NgayBatDau;
                    b.NgayKetThuc = _ctsp.NgayKetThuc;
                    b.GiaTriGiam = _ctsp.GiaTriGiam;
                    b.SoLuong = _ctsp.SoLuong;
                    b.MoTa = _ctsp.MoTa;
                    b.TrangThai = _ctsp.TrangThai;
                    return await _irespon.UpdateItem(b);
                }
            }
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateStatusVoucher(string id, int? TrangThai)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaVoucher == id);
            if (b != null)
            {
                b.TrangThai = TrangThai;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> DeleteVcher(string id)
        {
            var lstv = await _irespon.GetAll();
            var v = lstv.FirstOrDefault(x => x.MaVoucher == id);
            if (v == null)
            {
                return false;
            }
            else
            {
                return await _irespon.DeleteItem(v);
            }
        }
    }
}
