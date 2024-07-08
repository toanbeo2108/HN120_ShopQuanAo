using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        public GioHangController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _response = new AllResponsitories<GioHang>(_context, _context.GioHang);
            _responses = new AllResponsitories<GioHangChiTiet>(_context, _context.GioHangChiTiet);
        }
        // Thêm phương thức mới để lấy Giỏ Hàng theo UserId
        [HttpGet("GetGioHangByUserId/{userId}")]
        public async Task<ActionResult<GioHang>> GetGioHangByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId); // Tìm User theo UserId
            if (user == null)
            {
                return NotFound("User not found"); // Nếu không tìm thấy User, trả về NotFound
            }

            var gioHang = await _context.GioHang.Include(g => g.User).FirstOrDefaultAsync(g => g.User.Id == userId); // Tìm Giỏ Hàng của User
            if (gioHang == null)
            {
                return NotFound("GioHang not found"); // Nếu không tìm thấy Giỏ Hàng, trả về NotFound
            }

            return Ok(gioHang); // Trả về Giỏ Hàng nếu tìm thấy
        }


        [HttpGet("[Action]/{UserId}")]
        public async Task<List<GioHang>> GetGHByUserId(string UserId)
        {
            return await _context.GioHang.Where(x => x.User.Id == UserId).ToListAsync();
        }

        [HttpPost("[Action]")]
        public async Task<bool> CreateGio(string UserId, decimal? TongTien, int? MoTa, int? TrangThai)
        {
            GioHang GH = new GioHang();
            GH.MaGioHang = Guid.NewGuid().ToString();

            // Lấy thông tin user từ UserManager
            var user = await _userManager.FindByIdAsync(UserId);
            if (user != null)
            {
                GH.User = user;
                GH.TongTien = TongTien;
                GH.MoTa = MoTa;
                GH.TrangThai = TrangThai;
                return await _response.CreateItem(GH);
            }
            else
            {
                // Xử lý khi không tìm thấy user
                return false;
            }
        }

        [HttpPut("UpdateGHCT/{MaGH}")]
        public async Task<bool> UpdateGH(string MaGH, [FromBody] GioHang GH)
        {
            var gh = await _context.GioHang.FindAsync(MaGH);
            if (gh != null)
            {
                // Lấy thông tin user từ UserManager
                var user = await _userManager.FindByIdAsync(GH.User.Id);
                if (user != null)
                {
                    gh.User = user;
                    gh.TongTien = GH.TongTien;
                    gh.MoTa = GH.MoTa;
                    gh.TrangThai = GH.TrangThai;

                    return await _response.UpdateItem(gh);
                }
                else
                {
                    // Xử lý khi không tìm thấy user
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        [HttpDelete("[Action]/{MaGH}")]
        public async Task<bool> DeleteGH(string MaGH)
        {
            var gh = await _context.GioHang.FindAsync(MaGH);
            if (gh != null)
            {
                _context.GioHang.Remove(gh);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}