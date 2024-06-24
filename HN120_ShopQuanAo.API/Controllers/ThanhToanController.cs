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
    public class ThanhToanController : ControllerBase
    {
        private readonly IThanhToanServices _sv;
        public ThanhToanController(IThanhToanServices sv)
        {
           _sv = sv;
        }
        [HttpGet("[Action]")]
        public IActionResult GetAllThanhToan()
        {
            return  Ok(_sv.GetAllThanhtoan());
        }
        [HttpGet("[Action]/{ma}")]
        public IActionResult GetTTByma(string ma)
        {
            try
            {
                var tt = _sv.GetThanhToanByMa(ma);
                return Ok(tt);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("[Action]")]
        public IActionResult CreateThanhToan(ThanhToan tt)
        {
            try
            {
                _sv.CreatThanhToan(tt);
                return StatusCode(StatusCodes.Status201Created);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("[Action]")]
        public IActionResult UpdateHoaDon(ThanhToan tt)
        {
            try
            {
                _sv.UpdateThanhToan(tt);
                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("[Action]/{ma}")]
        public IActionResult DeleteThanhToan(string ma)
        {
            try
            {

                _sv.DeleteThanhToan(ma);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
