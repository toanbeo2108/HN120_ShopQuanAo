using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/Size")]
    [ApiController]
    public class SizeController : Controller
    {
        private readonly IAllResponsitories<ChiTietSp> _iresponCTSP;
        private readonly IAllResponsitories<Size> _irespon;
        AppDbContext _context = new AppDbContext();
        public SizeController()
        {
            _iresponCTSP = new AllResponsitories<ChiTietSp>(_context, _context.ChiTietSp);
            _irespon = new AllResponsitories<Size>(_context, _context.Size);

        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Size>> GetAllSize()
        {
            return await _irespon.GetAll();
        }
        [HttpGet("GetSZByID/{id}")]
        public async Task<Size> GetMSById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("add-SZ")]
        public async Task<bool> AddSZ(string Masz, string? Tensz, string? MoTa, int? TrangThai)
        {
            Size b = new Size();
            b.MaSize = Masz;
            b.TenSize = Tensz;
            b.MoTa = MoTa;
            b.TrangThai = TrangThai;
            return await _irespon.CreateItem(b);
        }
        [HttpPut("update-SZ/{id}")]
        public async Task<bool> UpdateSZ(string id, [FromBody] Size _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaSize == id);
            if (b != null)
            {

                b.TenSize = _ctsp.TenSize;
                b.MoTa = _ctsp.MoTa;
                b.TrangThai = _ctsp.TrangThai;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("delete-sz/{id}")]
        public async Task<bool> deleteSZ(string id)
        {
            var lstsp = await _irespon.GetAll();
            var ms = lstsp.FirstOrDefault(c => c.MaSize == id);

            if (ms != null)
            {
                var lstspct = await _iresponCTSP.GetAll();
                var dsspct = lstspct.Where(pd => pd.MaSize == ms.MaSize).ToList();
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
