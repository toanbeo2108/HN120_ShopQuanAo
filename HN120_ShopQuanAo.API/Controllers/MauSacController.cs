using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/MauSac")]
    [ApiController]
    public class MauSacController : ControllerBase
    {
        private readonly IAllResponsitories<MauSac> _irespon;
        private readonly IAllResponsitories<ChiTietSp> _iresponCTSP;
        AppDbContext _context = new AppDbContext();
        public MauSacController()
        {
            _irespon = new AllResponsitories<MauSac>(_context, _context.MauSac);
            _iresponCTSP = new AllResponsitories<ChiTietSp>(_context, _context.ChiTietSp);

        }
        // GET: api/<MauSacController>
        [HttpGet("[Action]")]
        public async Task<IEnumerable<MauSac>> GetAllMauSac()
        {
            return await _irespon.GetAll();
        }
        [HttpGet("[Action]/{id}")]
        public async Task<MauSac> GetMSById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("[Action]")]
        public async Task<bool> AddMS(string? TenMau, string? MoTa)
        {
            var mausacs = await GetAllMauSac();
            int msCount = mausacs.Count() + 1;
            MauSac b = new MauSac();
            b.MaMau = "MS" + msCount.ToString();
            b.TenMau = TenMau;
            b.MoTa = MoTa;
            b.TrangThai = 1;
            return await _irespon.CreateItem(b);
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> EditMS(string id, [FromBody] MauSac _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaMau == id);
            if (b != null)
            {

                b.TenMau = _ctsp.TenMau;
                b.MoTa = _ctsp.MoTa;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateStatusMauSac(string id, int? _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaMau == id);
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
        public async Task<bool> deleteMS(string id)
        {
            var lstsp = await _irespon.GetAll();
            var ms = lstsp.FirstOrDefault(c => c.MaMau == id);

            if (ms != null)
            {
                var lstspct = await _iresponCTSP.GetAll();
                var dsspct = lstspct.Where(pd => pd.MaMau == ms.MaMau).ToList();
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
