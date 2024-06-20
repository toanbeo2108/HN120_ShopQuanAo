using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/SanPham")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly IAllResponsitories<SanPham> _irespon;
        private readonly IAllResponsitories<ChiTietSp> _iresponCTSP;

        AppDbContext _context = new AppDbContext();
        public SanPhamController()
        {
            _irespon = new AllResponsitories<SanPham>(_context, _context.SanPham);
            _iresponCTSP = new AllResponsitories<ChiTietSp>(_context, _context.ChiTietSp);


        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<SanPham>> GetAllSanPham()
        {
            return await _irespon.GetAll();
        }
        [HttpGet("[Action]/{id}")]
        public async Task<SanPham> GetSPById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("[Action]")]
        public async Task<bool> AddSP(string? Tensp, string? MaThuongHieu, string? MaTheLoai, string? MaChatLieu, string? MoTa, string? UrlAvatar)
        {
            var sizes = await GetAllSanPham();
            int szCount = sizes.Count() + 1;
            SanPham b = new SanPham();
            b.MaSp = "SP" + szCount.ToString();
            b.TenSP = Tensp;
            b.MaThuongHieu = MaThuongHieu;
            b.MaTheLoai = MaTheLoai;
            b.MaChatLieu = MaChatLieu;

            b.TongSoLuong = 0;
            b.Mota = MoTa;
            b.NgayNhap = DateTime.Now;
            b.TrangThai = 1;
            b.UrlAvatar = UrlAvatar;
            return await _irespon.CreateItem(b);
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateSP(string id, [FromBody] SanPham _sp)
        {

            var sp = await _irespon.GetAll();
            var b = sp.FirstOrDefault(c => c.MaSp == id);
            if (b != null)
            {

                b.TenSP = _sp.TenSP;
                b.MaTheLoai = _sp.MaTheLoai;
                b.MaThuongHieu = _sp.MaThuongHieu;
                b.Mota = _sp.Mota;
                b.TrangThai = _sp.TrangThai;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateQuantitySanPham(string id)
        {

            var sp = await _irespon.GetAll();
            var b = sp.FirstOrDefault(c => c.MaSp == id);
            var ctsp = await _iresponCTSP.GetAll();
            var sp_ctsp = ctsp.Where(c => c.MaSp == id);
            if (b != null)
            {
                b.TongSoLuong = sp_ctsp.Sum(p => p.SoLuongTon);
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]")]
        public async Task<bool> UpdateQuantityAllSanPham()
        {
            var allsp = await _irespon.GetAll();
            var ctsp = await _iresponCTSP.GetAll();
            foreach (var item in allsp)
            {
                var masp = item.MaSp;

                var sp_ctsp = ctsp.Where(c => c.MaSp == masp);
                item.TongSoLuong = sp_ctsp.Sum(p => p.SoLuongTon);
                await _irespon.UpdateItem(item);
            }
            return true;
        }

        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateStatusSanPhamKD(string id)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaSp == id);
            if (b != null)
            {
                b.TrangThai = 1;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateStatusSanPhamKKD(string id)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaSp == id);
            if (b != null)
            {
                b.TrangThai = 0;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> deleteSP(string id)
        {
            var lstsp = await _irespon.GetAll();
            var sp = lstsp.FirstOrDefault(c => c.MaSp == id);

            if (sp != null)
            {
                var lstspct = await _iresponCTSP.GetAll();
                var dsspct = lstspct.Where(pd => pd.MaSp == sp.MaSp).ToList();
                foreach (var t in dsspct)
                {
                    await _iresponCTSP.DeleteItem(t);
                }
                return await _irespon.DeleteItem(sp);
            }
            else
            {
                return false;
            }
        }
    }
}
