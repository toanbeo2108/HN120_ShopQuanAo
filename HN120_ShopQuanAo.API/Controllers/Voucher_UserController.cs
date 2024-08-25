using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Voucher_UserController : ControllerBase
    {
        private readonly IAllResponsitories<User_Voucher> _iresponse;
        private readonly IHoaDonResponse _irespon;
        AppDbContext _appDbContext = new AppDbContext();
        public Voucher_UserController()
        {
            _iresponse = new AllResponsitories<User_Voucher>(_appDbContext,_appDbContext.User_Voucher);
            _irespon = new HoaDonResponse();
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<User_Voucher>> GetAllUser_Voucher()
        {
            return await _iresponse.GetAll();
        }
        [HttpGet("[Action]/{mand}")]
        public async Task<IEnumerable<User_Voucher>> GetVoucher_UserbyUserId(string mand)
        {
            return await _irespon.GetVoucherbyUserid(mand);
        }
        [HttpPut("[Action]/{mauvc}")]
        public async Task<bool> UpdateMaUVC (string mauvc)
        {
            return await _irespon.UpdateUser_Voucher(mauvc);
        }
        [HttpPost("CreateUVC")]
        public async Task<bool> CreateUVC(string userid, string mavc)
        {
            return await _irespon.CreateUVC(userid, mavc);  
        }
        [HttpPut("[Action]/{userid}&{mavc}")]
        public async Task<bool> UpdateUVCByUserIdMavc (string userid,string mavc)
        {
            return await _irespon.UpdateVoucherUserByUserIdMaVoucher(userid, mavc);
        }

    }
}
