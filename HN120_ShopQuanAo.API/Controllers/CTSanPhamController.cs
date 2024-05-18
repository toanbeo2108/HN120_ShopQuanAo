using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.Ireponsitory;
using HN120_ShopQuanAo.API.Repository;
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
        [HttpGet("GetCTSPByID/{id}")]
        public async Task<ChiTietSp> GetCTSPById(string id)
        {
            return await _iresponCTSP.GetByID(id);
        }
        [HttpPost("add-CTSP")]
        public async Task<bool> AddCTSP(string sKU, string? MaSp, string? MaSize, string? MaMau, string? MaTheLoai, decimal? GiaNhap, decimal? GiaBan, int? SoLuongTon, int? TrangThai)
        {
            ChiTietSp b = new ChiTietSp();
            b.SKU = sKU;
            b.MaSp = MaSp;
            b.MaMau = MaMau;
            b.MaSize = MaSize;
            b.MaTheLoai = MaTheLoai;
            b.GiaNhap = GiaNhap;
            b.GiaBan = GiaBan;
            b.SoLuongTon = SoLuongTon;
            b.TrangThai = TrangThai;
            return await _iresponCTSP.CreateItem(b);


        }
        [HttpPut("update-CTSP/{id}")]
        public async Task<bool> UpdateCTSP(string id, [FromBody] ChiTietSp _ctsp)
        {
            var ctsp = await _iresponCTSP.GetAll();
            var b = ctsp.FirstOrDefault(c => c.SKU == id);
            if (b != null)
            {

                b.MaSp = _ctsp.MaSp;
                b.MaMau = _ctsp.MaMau;
                b.MaTheLoai = _ctsp.MaTheLoai;
                b.MaSize = _ctsp.MaSize;
                b.GiaBan = _ctsp.GiaBan;
                b.GiaNhap = _ctsp.GiaNhap;
                b.SoLuongTon = _ctsp.SoLuongTon;
                b.TrangThai = _ctsp.TrangThai;
                return await _iresponCTSP.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("delete-CTSP/{id}")]
        public async Task<bool> deleteBook(string id)
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
