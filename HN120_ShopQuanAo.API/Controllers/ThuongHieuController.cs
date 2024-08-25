using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/ThuongHieu")]
    [ApiController]
    public class ThuongHieuController : ControllerBase
    {
        private readonly IAllResponsitories<ThuongHieu> _irespon;
        private readonly IAllResponsitories<SanPham> _iresponSP;

        AppDbContext _context = new AppDbContext();
        public ThuongHieuController()
        {
            _irespon = new AllResponsitories<ThuongHieu>(_context, _context.ThuongHieu);
            _iresponSP = new AllResponsitories<SanPham>(_context, _context.SanPham);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<ThuongHieu>> GetAllThuongHieu()
        {
            return await _irespon.GetAll();
        }
        // GET: TheLoaiController
        [HttpGet("[Action]/{id}")]
        public async Task<ThuongHieu> GetTHById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("[Action]")]
        public async Task<bool> AddThuongHieu(string? Tenth, string? MoTa)
        {
            var thuonghieus = await GetAllThuongHieu();
            int thCount = thuonghieus.Count() + 1;
            ThuongHieu b = new ThuongHieu();
            b.MaThuongHieu = "TH" + thCount.ToString();
            b.TenThuongHieu = Tenth;
            b.MoTa = MoTa;
            b.TrangThai = 1;
            return await _irespon.CreateItem(b);
        }
        //[HttpGet]
        //public IActionResult GetThuongHieus()
        //{
        //    var thuongHieus = _context.ThuongHieu.Select(th => new {
        //        th.MaThuongHieu,
        //        th.TenThuongHieu
        //    }).ToList();
        //    return Json(thuongHieus);
        //}
        [HttpPut("[Action]/{id}")]
        public async Task<bool> EditThuongHieu(string id, [FromBody] ThuongHieu _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaThuongHieu == id);
            if (b != null)
            {

                b.TenThuongHieu = _ctsp.TenThuongHieu;
                b.MoTa = _ctsp.MoTa;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateStatusThuongHieu(string id, int? _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaThuongHieu == id);
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
        public async Task<bool> deleteThuongHieu(string id)
        {
            var lstsp = await _irespon.GetAll();
            var ms = lstsp.FirstOrDefault(c => c.MaThuongHieu == id);

            if (ms != null)
            {
                var lstspct = await _iresponSP.GetAll();
                var dsspct = lstspct.Where(pd => pd.MaThuongHieu == ms.MaThuongHieu).ToList();
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
