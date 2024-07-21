using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/GioHangChiTiet")]
    [ApiController]
    public class GioHangChiTietController : ControllerBase
    {
        private readonly IAllResponsitories<GioHangChiTiet> _response;
        private readonly IGHCTResponse _iresponse;
         AppDbContext _context = new AppDbContext() ;
        public GioHangChiTietController()
        {
            _response = new AllResponsitories<GioHangChiTiet>(_context, _context.GioHangChiTiet);
            _iresponse = new GHCTResponse();
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<GioHangChiTiet>> AllGioHangChiTiet()
        {
            return await _response.GetAll();
        }   
        [HttpGet("[Action]/{MaGH}")]
        public async Task<List<GioHangChiTiet>> GetGHCTByMaGH(string MaGH)
        {
            return await _context.GioHangChiTiet.Where(x => x.MaGioHang == MaGH).ToListAsync();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateGioHangChiTiet( string? MaGH,string? SKU, string? TenSP,decimal? DonGia,int? SoLuong,int? TrangThai)
        {
            GioHangChiTiet GHCT = new GioHangChiTiet();
            GHCT.MaGioHangChiTiet = Guid.NewGuid().ToString();
            GHCT.MaGioHang = MaGH;
            GHCT.SKU = SKU;
            GHCT.TenSp = TenSP;
            GHCT.DonGia = DonGia;
            GHCT.SoLuong = SoLuong;
            GHCT.ThanhTien = DonGia*SoLuong;
            GHCT.TrangThai = TrangThai;
            return await _response.CreateItem(GHCT);
        }

        [HttpPut("[Action]/{MaGHCT}")]
        public async Task<bool> UptdateGHCTAll(string MaGHCT, [FromBody] GioHangChiTiet GHCT)
        {
            var listGHCT = await _response.GetAll();
            var ghct = listGHCT.FirstOrDefault(x => x.MaGioHangChiTiet == MaGHCT);
            if(ghct != null)
            {
                ghct.MaGioHang = GHCT.MaGioHang;
                ghct.SKU = GHCT.SKU;
                ghct.TenSp = GHCT.TenSp;
                ghct.DonGia = GHCT.DonGia;
                ghct.SoLuong = GHCT.SoLuong;
                ghct.ThanhTien = GHCT.ThanhTien;
                ghct.TrangThai = GHCT.TrangThai;
                return await _response.UpdateItem(ghct);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]/{MaGHCT}")]
        public async Task<bool> UpdateGHCT(string MaGHCT, int? soluong)
        {
           return await _iresponse.UpdateGHCT(MaGHCT, soluong);
        }
        [HttpDelete("[Action]/{MaGHCT}")]

        public async Task<bool> DeleteGHCT(string MaGHCT)
        {
            var list = await _response.GetAll();
            var c = list.FirstOrDefault(c => c.MaGioHangChiTiet == MaGHCT);
            if (c != null)
            {

                return await _response.DeleteItem(c);
            }
            else
            {
                return false;
            }
        }

    }
}
