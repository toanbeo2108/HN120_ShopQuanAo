using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietHoaDonController : ControllerBase
    {
        private readonly IChiTietHoaDonService _sv;
        public ChiTietHoaDonController(IChiTietHoaDonService sv)
        {
                _sv = sv;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
             var hdct =_sv.GetAllHoaDonChiTiet();
            return Ok(hdct);
        }
        [HttpGet("ma:string")]
        public IActionResult GetByMa(string ma) 
        {
            try
            {
               var hdct = _sv.GetHoaDonChiTietByMa(ma);
                return Ok(hdct);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
         [HttpPost]
        public IActionResult CreateHDCT(HoaDonChiTiet hoaDonCT) 
        {
            try
            {
                _sv.CreateCTHD(hoaDonCT);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
         [HttpPut]
        public IActionResult Update(HoaDonChiTiet hdct) 
        {
            try
            {
                _sv.UpdateCTHD(hdct);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
         [HttpDelete("ma:string")]
        public IActionResult Delete(string ma) 
        {
            try
            {
                _sv.DeleteCTHD(ma);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
