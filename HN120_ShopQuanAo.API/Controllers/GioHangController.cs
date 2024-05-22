using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.Ireponsitory;
using HN120_ShopQuanAo.API.Repository;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/GioHang")]
    [ApiController]
    public class GioHangController : ControllerBase
    {
        private readonly IAllResponsitories<GioHang> _response;
        private readonly IAllResponsitories<GioHangChiTiet> _responses;
        AppDbContext _context = new AppDbContext();
        public GioHangController()
        {
            _response = new AllResponsitories<GioHang>(_context, _context.GioHang);
            _responses = new AllResponsitories<GioHangChiTiet>(_context, _context.GioHangChiTiet);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<GioHang>> AllGioHang()
        {
            return await _response.GetAll();
        }
        [HttpGet("[Action]/{UserId}")]
        public async Task<List<GioHang>> GetGHByUserId(string UserId)
        {
            return await _context.GioHang.Where(x => x.UserID == UserId).ToListAsync();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateGio(string? UserId, decimal? TongTien, int? MoTa, int? TrangThai)
        {
            GioHang GH = new GioHang();
            GH.MaGioHang = Guid.NewGuid().ToString();
            GH.UserID = UserId;
            GH.TongTien = TongTien;
            GH.MoTa = MoTa;
            GH.TrangThai = TrangThai;
            return await _response.CreateItem(GH);
        }

        [HttpPut("UpdateGHCT/{MaGH}")]
        public async Task<bool> UptdateGH(string MaGH, [FromBody] GioHang GH)
        {
            var listGH = await _response.GetAll();
            var gh = listGH.FirstOrDefault(x => x.MaGioHang == MaGH);
            if (gh != null)
            {
                gh.UserID = GH.UserID;
                gh.TongTien = GH.TongTien;
                gh.MoTa = GH.MoTa;
                gh.TrangThai = GH.TrangThai;
              
                return await _response.UpdateItem(gh);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("[Action]/{MaGH}")]

        public async Task<bool> DeleteGH(string MaGH)
        {
            var list = await _response.GetAll();
            var c = list.FirstOrDefault(c => c.MaGioHang == MaGH);
            if (c != null)
            {
                var ListGHCT = _context.GioHangChiTiet.Where(x => x.MaGioHang == c.MaGioHang);
                if(ListGHCT != null)
                {
                    foreach( var x in ListGHCT)
                    {
                       await _responses.DeleteItem(x);
                    }
                }
                else
                {
                    return await _response.DeleteItem(c);
                }
                return await _response.DeleteItem(c);
            }
            else
            {
                return false;
            }
        }
    }
}
