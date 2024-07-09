using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Configurations;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichSuHoaDonController : ControllerBase
    {

        private readonly LichSuHoaDon_IService _sv;
        public LichSuHoaDonController(LichSuHoaDon_IService sv)
        {
            _sv = sv;
        }
        [HttpGet("[Action]")]
        public IActionResult GetAll()
        {
            var lshd = _sv.GetAllLichSuHoaDon();
            return Ok(lshd);
        }
        [HttpPost("[Action]")]
        public IActionResult CreateHDCT(HoaDon_History hoaDonhistorry)
        {
            try
            {
                _sv.CreateLichSuHoaDon(hoaDonhistorry);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
