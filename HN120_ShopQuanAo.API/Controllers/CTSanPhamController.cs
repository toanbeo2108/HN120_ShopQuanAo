using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/CTSanPham")]
    [ApiController]
    public class CTSanPhamController : ControllerBase
    {

        private readonly IAllResponsitories<ChiTietSp> _iresponCTSP;
        //private readonly IAllResponsitories<Size> _iresponSZ;

        AppDbContext _context = new AppDbContext();
        public CTSanPhamController()
        {
            _iresponCTSP = new AllResponsitories<ChiTietSp>(_context, _context.ChiTietSp);

            //_iresponSZ = new AllResponsitories<Size>(_context, _context.Size);

        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<ChiTietSp>> GetAllCTSanPham()
        {
            return await _iresponCTSP.GetAll();
        }
        [HttpGet("[Action]/{id}")]
        public async Task<ChiTietSp> GetCTSPById(string id)
        {
            return await _iresponCTSP.GetByID(id);
        }
        [HttpPost("[Action]")]
        public async Task<bool> AddCTSP(string? MaSp, string? MaSize, string? MaMau, string? MaKhuyenMai, string? MaChatLieu, decimal? GiaBan, int? SoLuongTon)
        {
            ChiTietSp b = new ChiTietSp();

            b.MaSp = MaSp;
            b.MaMau = MaMau;
            b.MaSize = MaSize;
            b.MaKhuyenMai = MaKhuyenMai;
            b.MaChatLieu = MaChatLieu;
            b.SKU = MaSp + MaMau + MaSize + MaKhuyenMai + MaChatLieu;
            b.GiaBan = GiaBan;
            b.SoLuongTon = SoLuongTon;
            if (b.SoLuongTon == 0)
            {
                b.TrangThai = 0;
            }
            if (b.SoLuongTon > 0)
            {
                b.TrangThai = 1;
            }
            return await _iresponCTSP.CreateItem(b);
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateCTSP(string id, [FromBody] ChiTietSp _ctsp)
        {
            var ctsp = await _iresponCTSP.GetAll();
            var b = ctsp.FirstOrDefault(c => c.SKU == id);
            if (b != null)
            {


                b.MaKhuyenMai = _ctsp.MaKhuyenMai;
                b.MaChatLieu = _ctsp.MaChatLieu;
                b.GiaBan = _ctsp.GiaBan;
                b.SoLuongTon = _ctsp.SoLuongTon;
                if (b.SoLuongTon == 0)
                {
                    b.TrangThai = 0;
                }
                if (b.SoLuongTon > 0)
                {
                    b.TrangThai = 1;
                }

                return await _iresponCTSP.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateStatusCTSanPham(string id, int? _ctsp)
        {
            var ctsp = await _iresponCTSP.GetAll();
            var b = ctsp.FirstOrDefault(c => c.SKU == id);
            if (b != null)
            {
                b.TrangThai = _ctsp;
                return await _iresponCTSP.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> deleteCTSP(string id)
        {
            var listBook = await _iresponCTSP.GetAll();
            var re = listBook.FirstOrDefault(c => c.SKU == id);
            if (re != null)
            {
                return await _iresponCTSP.DeleteItem(re);
            }
            else
            {
                return false;
            }
        }
    }
}
