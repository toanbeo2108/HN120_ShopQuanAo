using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
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
        private readonly ILogger<SanPhamController> _logger;

        AppDbContext _context = new AppDbContext();
        public SanPhamController(ILogger<SanPhamController> logger)
        {
            _irespon = new AllResponsitories<SanPham>(_context, _context.SanPham);
            _iresponCTSP = new AllResponsitories<ChiTietSp>(_context, _context.ChiTietSp);
            _logger = logger;
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
        [HttpPost("AddSpWithDetails")]
        public async Task<IActionResult> AddSpWithDetails([FromBody] AddSpViewModel model)
        {

            var listsp = await _irespon.GetAll();
            int spcount = listsp.Count() + 1;
            // Tạo sản phẩm mới
            var newProduct = new SanPham
            {
                MaSp = "SP_" + spcount.ToString(),
                TenSP = model.TenSp,
                MaThuongHieu = model.MaThuongHieu,
                MaTheLoai = model.MaTheLoai,
                MaChatLieu = model.MaChatLieu,
                UrlAvatar = model.UrlAvatar,
                NgayNhap = DateTime.Now,
                TrangThai = 1
            };



            // Thêm chi tiết sản phẩm
            foreach (var detail in model.ChiTietSps)
            {
                var newChiTietSp = new ChiTietSp
                {
                    MaSp = newProduct.MaSp,
                    MaSize = detail.MaSize,
                    MaMau = detail.MaMau,
                    DonGia = detail.DonGia,
                    GiaBan = detail.DonGia,
                    SoLuongTon = detail.SoLuongTon,
                    UrlAnhSpct = detail.UrlAnhSpct,
                    SKU = newProduct.MaSp + "_" + detail.MaSize + "_" + detail.MaMau,
                    TrangThai = 1
                };

                await _iresponCTSP.CreateItem(newChiTietSp);
            }
            newProduct.TongSoLuong = model.ChiTietSps.Sum(c => c.SoLuongTon);
            await _irespon.CreateItem(newProduct);
            return Ok(new { success = true, message = "Product and details added successfully" });


            return BadRequest(ModelState);
        }


        [HttpPut("[Action]/{id}")]
        public async Task<bool> EditSP(string id, [FromBody] SanPham _sp)
        {

            var sp = await _irespon.GetAll();
            var b = sp.FirstOrDefault(c => c.MaSp == id);
            if (b != null)
            {

                b.TenSP = _sp.TenSP;
                b.MaTheLoai = _sp.MaTheLoai;
                b.MaThuongHieu = _sp.MaThuongHieu;
                b.UrlAvatar = _sp.UrlAvatar;
                b.Mota = _sp.Mota;
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
        public async Task<bool> UpdateStatusSanPham(string id, int _sp)
        {
            var lstsp = await _irespon.GetAll();
            var sp = lstsp.FirstOrDefault(c => c.MaSp == id);
            var lstctsp = await _iresponCTSP.GetAll();
            var ctsp_sp = lstctsp.Where(c => c.MaSp == id).ToList();
            if (sp != null)
            {
                foreach (var ctsp in ctsp_sp)
                {
                    ctsp.TrangThai = _sp;
                    await _iresponCTSP.UpdateItem(ctsp);
                }
                sp.TrangThai = _sp;

                return await _irespon.UpdateItem(sp);
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
