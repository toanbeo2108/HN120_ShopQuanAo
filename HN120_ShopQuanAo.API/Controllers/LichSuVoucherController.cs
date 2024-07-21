using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    public class LichSuVoucherController : Controller
    {
        private readonly IAllResponsitories<VoucherHistory> _irespon;
        AppDbContext _context = new AppDbContext();
        public LichSuVoucherController()
        {
            _irespon = new AllResponsitories<VoucherHistory>(_context, _context.VoucherHistory);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<VoucherHistory>> GetAllVoucherHistory()
        {
            return await _irespon.GetAll();
        }
        [HttpGet("[Action]/{id}")]
        public async Task<VoucherHistory> GetVCHById(string id)
        {
            return await _irespon.GetByID(id);
        }
    }
}
