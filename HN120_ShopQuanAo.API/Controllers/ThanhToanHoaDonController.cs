using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanHoaDonController : ControllerBase
    {
        private readonly IThanhToanHoaDonService _sv;
        public ThanhToanHoaDonController(IThanhToanHoaDonService sv)
        {
            _sv = sv;
        }
        [HttpGet("[Action]")]
        public IActionResult GetAllThanhToan_HoaDon()
        {
            return Ok( _sv.GetAllThanhToan_HoaDon());
        }
        [HttpGet("[Action]/{ma}")]
        public IActionResult GetThanhToan_HoaDonById(string ma)
        {
            try
            {
                
                return Ok(_sv.GetThanhToan_HoaDonByMa(ma));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("[Action]")]
        public IActionResult AddThanhToanThanhToan_HoaDon(ThanhToan_HoaDon tt)
        {
            try
            {
                _sv.CreateThanhToan_HoaDon(tt);
                return StatusCode(StatusCodes.Status201Created);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("[Action]")]
        public IActionResult UpdateThanhToan_HoaDon(ThanhToan_HoaDon tt)
        {
            try
            {
                _sv.UpdateThanhToan_HoaDon(tt);
                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("[Action]/{ma}")]
        public IActionResult deleteThanhToan_HoaDon(string ma)
        {
            try
            {

                _sv.DeleteThanhToan_HoaDon(ma);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
   
}

