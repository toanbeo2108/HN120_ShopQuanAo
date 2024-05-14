using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.Ireponsitory;
using HN120_ShopQuanAo.API.Repository;
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
        private readonly IAllResponsitories<ChiTietSp> _iresponCTSP;
        AppDbContext _context = new AppDbContext();
        public TheLoaiController()
        {
            _irespon = new AllResponsitories<TheLoai>(_context, _context.TheLoai);
            _iresponCTSP = new AllResponsitories<ChiTietSp>(_context, _context.ChiTietSp);

        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<TheLoai>> GetAllTheLoai()
        {
            return await _irespon.GetAll();
        }
        // GET: TheLoaiController
        [HttpGet("GetTLByID/{id}")]
        public async Task<TheLoai> GetTLById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("add-TL")]
        public async Task<bool> AddTL(string Matl, string? Tentl, string? MoTa, int? TrangThai)
        {
            TheLoai b = new TheLoai();
            b.MaTheLoai = Matl;
            b.TenTheLoai = Tentl;
            b.MoTa = MoTa;
            b.TrangThai = TrangThai;
            return await _irespon.CreateItem(b);
        }
        [HttpPut("update-TL/{id}")]
        public async Task<bool> UpdateTL(string id, [FromBody] TheLoai _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaTheLoai == id);
            if (b != null)
            {

                b.TenTheLoai = _ctsp.TenTheLoai;
                b.MoTa = _ctsp.MoTa;
                b.TrangThai = _ctsp.TrangThai;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("delete-tl/{id}")]
        public async Task<bool> deleteTL(string id)
        {
            var lstsp = await _irespon.GetAll();
            var ms = lstsp.FirstOrDefault(c => c.MaTheLoai == id);

            if (ms != null)
            {
                var lstspct = await _iresponCTSP.GetAll();
                var dsspct = lstspct.Where(pd => pd.MaTheLoai == ms.MaTheLoai).ToList();
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
