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
        [HttpGet("[Action]/{id}")]
        public async Task<KhuyenMai> GetKMById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("[Action]")]
        public async Task<bool> AddKM(string? TenKhuyenMai, decimal PhanTramGiam)
        {
            var khuyenmais = await GetAllKhuyenMai();
            int kmCount = khuyenmais.Count() + 1;
            KhuyenMai b = new KhuyenMai();
            b.MaKhuyenMai = "KM" + kmCount.ToString();
            b.TenKhuyenMai = TenKhuyenMai;
            b.PhanTramGiam = PhanTramGiam;
            b.TrangThai = 1;
            return await _irespon.CreateItem(b);
        }
        [HttpPut("[Action]/{id}")]

        public async Task<bool> EditKM(string id, [FromBody] KhuyenMai _ctsp)

        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaKhuyenMai == id);
            if (b != null)
            {

                b.TenKhuyenMai = _ctsp.TenKhuyenMai;
                b.PhanTramGiam = _ctsp.PhanTramGiam;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateStatusKhuyenMai(string id, int? _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaKhuyenMai == id);
            if (b != null)
            {
                b.TrangThai = _ctsp;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> deleteKM(string id)
        {
            var lstsp = await _irespon.GetAll();
            var ms = lstsp.FirstOrDefault(c => c.MaKhuyenMai == id);

            if (ms != null)
            {
                var lstspct = await _iresponCTSP.GetAll();
                var dsspct = lstspct.Where(pd => pd.MaMau == ms.MaKhuyenMai).ToList();
                foreach (var t in dsspct)
                {
                    await _iresponCTSP.DeleteItem(t);
                }
                return await _irespon.DeleteItem(ms);
            }
            else
            {
                return false;
            }
        }
    }
}
