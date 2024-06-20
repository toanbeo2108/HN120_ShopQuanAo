using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/CTSanPham")]
    [ApiController]
    public class CTSanPhamController : ControllerBase
    {

        private readonly IAllResponsitories<ChiTietSp> _iresponCTSP;
        private readonly IAllResponsitories<SanPham> _iresponSP;
        private readonly IAllResponsitories<KhuyenMai> _iresponKM;

        //private readonly IAllResponsitories<Size> _iresponSZ;

        AppDbContext _context = new AppDbContext();
        public CTSanPhamController()
        {
            _iresponCTSP = new AllResponsitories<ChiTietSp>(_context, _context.ChiTietSp);
            _iresponSP = new AllResponsitories<SanPham>(_context, _context.SanPham);
            _iresponKM = new AllResponsitories<KhuyenMai>(_context, _context.KhuyenMai);

            //_iresponSZ = new AllResponsitories<Size>(_context, _context.Size);

        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<ChiTietSp>> GetAllCTSanPham()
        {
            return await _iresponCTSP.GetAll();
        }
        [HttpGet("[Action]/{id}")]
        public async Task<ChiTietSp> GetCTSPById(string id)
        {
            return await _iresponCTSP.GetByID(id);
        }
        [HttpPost("[Action]")]
        public async Task<bool> AddCTSP(string? MaSp, string? MaSize, string? MaMau, string? MaKhuyenMai,string? UrlAnhSpct, decimal? DonGia, int? SoLuongTon)
        {
            var lstsp = await _iresponSP.GetAll();
            var sp = lstsp.FirstOrDefault(c => c.MaSp == MaSp);
            var lstkm = await _iresponKM.GetAll();
            var km = lstkm.FirstOrDefault(c => c.MaKhuyenMai == MaKhuyenMai);
            decimal? ptkm = km.PhanTramGiam;
            var ctsp = await _iresponCTSP.GetAll();
            var lstspct = ctsp.Where(c => c.MaSp == sp.MaSp && c.TrangThai ==1);
            ChiTietSp b = new ChiTietSp();
            b.SKU = MaSp + MaMau + MaSize;
            b.MaSp = MaSp;
            b.MaMau = MaMau;
            b.MaSize = MaSize;
            b.MaKhuyenMai = MaKhuyenMai;
            b.UrlAnhSpct = UrlAnhSpct;
            b.DonGia = DonGia;
            if (b.MaKhuyenMai ==null || ptkm == 0)
            {
                b.GiaBan = DonGia;
            }
            else
            {
                b.GiaBan = DonGia - (DonGia*ptkm/100);
            }
            
            b.SoLuongTon = SoLuongTon;
            if (SoLuongTon == 0)
            {
                b.TrangThai = 0;
            }
            if (SoLuongTon > 0)
            {
                b.TrangThai = 1;
            }
            var check = await _iresponCTSP.CreateItem(b);
            sp.TongSoLuong = sp.TongSoLuong + SoLuongTon;
            var check2 = await _iresponSP.UpdateItem(sp);
            return check;
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateCTSP(string id, [FromBody] ChiTietSp _ctsp)
        {
            
            var ctsp = await _iresponCTSP.GetAll();
            var b = ctsp.FirstOrDefault(c => c.SKU == id);
            
            
            if (b != null)
            {


                
                b.GiaBan = _ctsp.GiaBan;
                b.SoLuongTon = _ctsp.SoLuongTon;
                if (b.SoLuongTon == 0)
                {
                    b.TrangThai = 0;
                }
                if (b.SoLuongTon > 0)
                {
                    b.TrangThai = 1;
                }
                
                await _iresponCTSP.UpdateItem(b);
                var lstsp = await _iresponSP.GetAll();
                var sp = lstsp.FirstOrDefault(c => c.MaSp == _ctsp.MaSp);
                var lstspct = ctsp.Where(c => c.MaSp == sp.MaSp && c.TrangThai ==1);
                sp.TongSoLuong = lstspct.Sum(c => c.SoLuongTon);
                return await _iresponSP.UpdateItem(sp);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateStatusCTSanPham(string id, int? _ctsp)
        {
            var ctsp = await _iresponCTSP.GetAll();
            var b = ctsp.FirstOrDefault(c => c.SKU == id);
            if (b != null)
            {
                b.TrangThai = _ctsp;
                return await _iresponCTSP.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> deleteCTSP(string id)
        {
            var listBook = await _iresponCTSP.GetAll();
            var re = listBook.FirstOrDefault(c => c.SKU == id);
            if (re != null)
            {
                return await _iresponCTSP.DeleteItem(re);
            }
            else
            {
                return false;
            }
        }
    }
}
