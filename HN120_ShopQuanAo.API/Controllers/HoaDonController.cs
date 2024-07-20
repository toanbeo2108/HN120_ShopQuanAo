using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.API.Service.Services;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDon_Service _sv;
        private readonly IHoaDonResponse _iresponse;
        public HoaDonController(IHoaDon_Service sv)
        {
            _iresponse = new HoaDonResponse();
            _sv = sv;
        }
        [HttpGet("[Action]")]
        public IActionResult GetAllHoaDon()
        {
            var hoadon = _sv.GetAllHoaDon();
            return Ok(hoadon);
        }
        [HttpGet("[Action]/{stt}")]
        public IActionResult GetAllHoaDonBySTT(int stt)
        {
            var hoadon = _sv.GetHoaDonByTrangthai(stt);
            return Ok(hoadon);
        }

        [HttpGet("[Action]/{ma}")]
        public IActionResult GetAllHoaDonMa(string ma)
        {
            try
            {
                var hoadon = _sv.GetHoaDonByMa(ma);
                return Ok(hoadon);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }
        [HttpGet("[Action]/{ma}")]
        public IActionResult GetHoaDonWithDetails(string ma)
        {

            var hoadon = _sv.GetHoaDonWithDetails(ma);
            return Ok(hoadon);
        }
        [HttpPost("[Action]")]
        public IActionResult CreateHoaDon(HoaDon hoaDon)
        {
            try
            {
                _sv.CreateHoaDon(hoaDon);
                return StatusCode(StatusCodes.Status201Created);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPut("[Action]")]
        public IActionResult UpdateHoaDon(HoaDon hoaDon)
        {
            try
            {
                _sv.UpdateHoaDon(hoaDon);
                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }
        [HttpDelete("[Action]/{ma}")]
        public IActionResult DeleteHoaDon(string ma)
        {
            try
            {

                _sv.DeleteHoaDon(ma);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateHoaDonUser(string UserId)
        {
            return await _iresponse.CreateHoaDonUser(UserId);
        }
        [HttpGet("[Action]/UserId")]
        public async Task<IEnumerable<HoaDon>> GetAllHDByUserId (string UserId)
        {
            return await _iresponse.GetAllHoaDonByUserId(UserId);
        }
        [HttpPut]
        public async Task<bool> UpdateHD(string maHD, string? MaVoucher, string? tenkh, string? sdt, decimal? phiship, decimal? tongtien, int? pttt, string? phanloai, string? ghichu, string? tinh, string? huyen, string? xa, string? cuthe)
        {
            return await _iresponse.UpdateHoaDon(maHD,MaVoucher, tenkh, sdt, phiship, tongtien, pttt, phanloai, ghichu, tinh, huyen, xa, cuthe);
        }
    }
}
