﻿using HN120_ShopQuanAo.API.Data;
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
        private readonly IAllResponsitories<SanPham> _iresponSP;
        AppDbContext _context = new AppDbContext();
        public ChatLieuController()
        {
            _irespon = new AllResponsitories<ChatLieu>(_context, _context.ChatLieu);
            _iresponSP = new AllResponsitories<SanPham>(_context, _context.SanPham);
        }

        // GET: api/<MauSacController>
        [HttpGet("[Action]")]
        public async Task<IEnumerable<ChatLieu>> GetAllChatLieu()
        {
            return await _irespon.GetAll();
        }
        [HttpGet("[Action]/{id}")]
        public async Task<ChatLieu> GetCLById(string id)
        {
            return await _irespon.GetByID(id);
        }
        [HttpPost("[Action]")]
        public async Task<bool> AddChatLieu(string? TenChatLieu, string? MoTa)
        {
            var lstchatlieu = await _irespon.GetAll();
            var cl = lstchatlieu.FirstOrDefault(x => x.TenChatLieu == TenChatLieu);

            int clCount = lstchatlieu.Count() + 1;
            if (cl != null)
            {
                return false;
            }
            ChatLieu b = new ChatLieu();
            b.MaChatLieu = "CL" + clCount.ToString();
            b.TenChatLieu = TenChatLieu;
            b.MoTa = MoTa;
            b.TrangThai = 1;

            return await _irespon.CreateItem(b);
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> EditChatLieu(string id, [FromBody] ChatLieu _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaChatLieu == id);
            if (b != null)
            {

                b.TenChatLieu = _ctsp.TenChatLieu;
                b.MoTa = _ctsp.MoTa;
                return await _irespon.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateStatusChatLieu(string id, int? _ctsp)
        {
            var ctsp = await _irespon.GetAll();
            var b = ctsp.FirstOrDefault(c => c.MaChatLieu == id);
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
        public async Task<bool> deleteChatLieu(string id)
        {
            var lstsp = await _irespon.GetAll();
            var ms = lstsp.FirstOrDefault(c => c.MaChatLieu == id);

            if (ms != null)
            {
                var lstspct = await _iresponSP.GetAll();
                var dsspct = lstspct.Where(pd => pd.MaChatLieu == ms.MaChatLieu).ToList();
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
