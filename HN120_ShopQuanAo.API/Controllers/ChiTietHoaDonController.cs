using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
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
        [HttpGet("[Action]")]
        public IActionResult GetAll()
        {
            var hdct = _sv.GetAllHoaDonChiTiet();
            return Ok(hdct);
        }
        [HttpGet("[Action]/{ma}")]
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
        [HttpPost("[Action]")]
        public IActionResult CreateHDCT([FromBody] List<HoaDonChiTiet> hoaDonCT)
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
        [HttpPut("[Action]")]
        public IActionResult Update([FromBody] List<HoaDonChiTiet> hdct)
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
        [HttpDelete("[Action]/{ma}")]
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
