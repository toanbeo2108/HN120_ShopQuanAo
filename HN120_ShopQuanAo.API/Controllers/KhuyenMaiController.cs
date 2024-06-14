using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IAllResponsitories<KhuyenMai> _irespon;
        private readonly IAllResponsitories<ChiTietSp> _iresponCTSP;

        AppDbContext _context = new AppDbContext();
        public KhuyenMaiController()
        {
            _irespon = new AllResponsitories<KhuyenMai>(_context, _context.KhuyenMai);
            _iresponCTSP = new AllResponsitories<ChiTietSp>(_context, _context.ChiTietSp);

        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<KhuyenMai>> GetAllKhuyenMai()
        {
            return await _irespon.GetAll();
        }
        // GET: TheLoaiController
        [HttpGet("GetTHByID/{id}")]
        public async Task<KhuyenMai> GetKMById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("add-TL")]
        public async Task<bool> AddKM(string? TenKhuyenMai, float? PhanTramGiam, int? TrangThai)
        {
            var khuyenmais = await GetAllKhuyenMai();
            int kmCount = khuyenmais.Count() + 1;
            KhuyenMai b = new KhuyenMai();
            b.MaKhuyenMai = "KM" + kmCount.ToString();
            b.TenKhuyenMai = TenKhuyenMai;
            b.PhanTramGiam = PhanTramGiam;
            b.TrangThai = TrangThai;
            return await _irespon.CreateItem(b);
        }
        [HttpPut("update-TL/{id}")]
        public async Task<bool> UpdateTL(string id, [FromBody] KhuyenMai _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaKhuyenMai == id);
            if (b != null)
            {

                b.TenKhuyenMai = _ctsp.TenKhuyenMai;
                b.PhanTramGiam = _ctsp.PhanTramGiam;
                b.TrangThai = _ctsp.TrangThai;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
    }
}
