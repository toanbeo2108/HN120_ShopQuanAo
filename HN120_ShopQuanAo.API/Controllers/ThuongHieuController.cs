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
        AppDbContext _context = new AppDbContext();
        public ThuongHieuController()
        {
            _irespon = new AllResponsitories<ThuongHieu>(_context, _context.ThuongHieu);

        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<ThuongHieu>> GetAllThuongHieu()
        {
            return await _irespon.GetAll();
        }
        // GET: TheLoaiController
        [HttpGet("GetTHByID/{id}")]
        public async Task<ThuongHieu> GetTHById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("add-TL")]
        public async Task<bool> AddTH(string? Tenth, string? MoTa, int? TrangThai)
        {
            var thuonghieus = await GetAllThuongHieu();
            int thCount = thuonghieus.Count() + 1;
            ThuongHieu b = new ThuongHieu();
            b.MaThuongHieu = "TH" + thCount.ToString();
            b.TenThuongHieu = Tenth;
            b.MoTa = MoTa;
            b.TrangThai = TrangThai;
            return await _irespon.CreateItem(b);
        }
        [HttpPut("update-TL/{id}")]
        public async Task<bool> UpdateTL(string id, [FromBody] ThuongHieu _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaThuongHieu == id);
            if (b != null)
            {

                b.TenThuongHieu = _ctsp.TenThuongHieu;
                b.MoTa = _ctsp.MoTa;
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
