using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/ChatLieu")]
    [ApiController]
    public class ChatLieuController : ControllerBase
    {
        private readonly IAllResponsitories<ChatLieu> _irespon;
        private readonly IAllResponsitories<ChiTietSp> _iresponCTSP;
        AppDbContext _context = new AppDbContext();
        public ChatLieuController()
        {
            _irespon = new AllResponsitories<ChatLieu>(_context, _context.ChatLieu);
            _iresponCTSP = new AllResponsitories<ChiTietSp>(_context, _context.ChiTietSp);
        }
      
        // GET: api/<MauSacController>
        [HttpGet("[Action]")]
        public async Task<IEnumerable<ChatLieu>> GetAllChatLieu()
        {
            return await _irespon.GetAll();
        }
        [HttpGet("GetMSByID/{id}")]
        public async Task<ChatLieu> GetCLById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("add-TH")]
        public async Task<bool> AddChatLieu(string? TenChatLieu, string? MoTa, int? TrangThai)
        {
            var chatlieus = await GetAllChatLieu();
            int clCount = chatlieus.Count() + 1;
            ChatLieu b = new ChatLieu();
            b.MaChatLieu = "TH" + clCount.ToString();
            b.TenChatLieu = TenChatLieu;
            b.MoTa = MoTa;
            b.TrangThai = TrangThai;
            return await _irespon.CreateItem(b);
        }
        [HttpPut("update-TH/{id}")]
        public async Task<bool> UpdateMS(string id, [FromBody] ChatLieu _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaChatLieu == id);
            if (b != null)
            {

                b.TenChatLieu = _ctsp.TenChatLieu;
                b.MoTa = _ctsp.MoTa;
                b.TrangThai = _ctsp.TrangThai;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }

        [HttpDelete("delete-TH/{id}")]
        public async Task<bool> deleteMS(string id)
        {
            var lstsp = await _irespon.GetAll();
            var ms = lstsp.FirstOrDefault(c => c.MaChatLieu == id);

            if (ms != null)
            {
                var lstspct = await _iresponCTSP.GetAll();
                var dsspct = lstspct.Where(pd => pd.MaChatLieu == ms.MaChatLieu).ToList();
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
