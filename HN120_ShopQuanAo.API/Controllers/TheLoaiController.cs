using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/TheLoai")]
    [ApiController]
    public class TheLoaiController : Controller
    {
        private readonly IAllResponsitories<TheLoai> _irespon;
        private readonly IAllResponsitories<SanPham> _iresponSP;
        AppDbContext _context = new AppDbContext();
        public TheLoaiController()
        {
            _irespon = new AllResponsitories<TheLoai>(_context, _context.TheLoai);
            _iresponSP = new AllResponsitories<SanPham>(_context, _context.SanPham);

        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<TheLoai>> GetAllTheLoai()
        {
            return await _irespon.GetAll();
        }
        // GET: TheLoaiController
        [HttpGet("[Action]/{id}")]
        public async Task<TheLoai> GetTLById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("[Action]")]
        public async Task<bool> AddTL(string? Tentl, string? MoTa)
        {
            var theloais = await GetAllTheLoai();
            int tlCount = theloais.Count() + 1;
            TheLoai b = new TheLoai();
            b.MaTheLoai = "TL" + tlCount.ToString();
            b.TenTheLoai = Tentl;
            b.MoTa = MoTa;
            b.TrangThai = 1;
            return await _irespon.CreateItem(b);
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> EditTL(string id, [FromBody] TheLoai _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaTheLoai == id);
            if (b != null)
            {

                b.TenTheLoai = _ctsp.TenTheLoai;
                b.MoTa = _ctsp.MoTa;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateStatusTheLoai(string id, int? _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaTheLoai == id);
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
        public async Task<bool> deleteTL(string id)
        {
            var lstsp = await _irespon.GetAll();
            var ms = lstsp.FirstOrDefault(c => c.MaTheLoai == id);

            if (ms != null)
            {
                var lstspct = await _iresponSP.GetAll();
                var dsspct = lstspct.Where(pd => pd.MaTheLoai == ms.MaTheLoai).ToList();
                foreach (var t in dsspct)
                {
                    await _iresponSP.DeleteItem(t);
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
