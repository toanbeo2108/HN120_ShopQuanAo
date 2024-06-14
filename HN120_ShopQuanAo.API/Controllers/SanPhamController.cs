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
        [HttpGet("GetSPByID/{id}")]
        public async Task<SanPham> GetSPById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("add-TL")]
        public async Task<bool> AddSP( string? Tensp,string? MaThuongHieu,string? MaTheLoai, string? MoTa, int? TongSoLuong, int? TrangThai, string? UrlAvatar)
        {
            var sizes = await GetAllSanPham();
            int szCount = sizes.Count() + 1;
            SanPham b = new SanPham();
            b.MaSp = "SP" + szCount.ToString();
            b.TenSP = Tensp;
            b.MaThuongHieu = MaThuongHieu;
            b.MaTheLoai = MaTheLoai;
            b.TongSoLuong = TongSoLuong;
            b.Mota = MoTa;
            b.TrangThai = TrangThai;
            b.UrlAvatar = UrlAvatar;
            return await _irespon.CreateItem(b);
        }
        [HttpPut("update-SP/{id}")]
        public async Task<bool> UpdateSP(string id, [FromBody] SanPham _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaSp == id);
            if (b != null)
            {

                b.TenSP = _ctsp.TenSP;
                b.TongSoLuong = _ctsp.TongSoLuong;
                b.Mota = _ctsp.Mota;
                b.TrangThai = _ctsp.TrangThai;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("delete-sp/{id}")]
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
