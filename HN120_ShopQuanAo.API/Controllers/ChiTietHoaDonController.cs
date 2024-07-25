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
    public class ChiTietHoaDonController : ControllerBase
    {
        private readonly IChiTietHoaDonService _sv;
        private readonly IHoaDonResponse _iresponse;
        private readonly IHoaDonChiTietResponse _iresponses;
        public ChiTietHoaDonController(IChiTietHoaDonService sv)
        {
            _iresponse = new HoaDonResponse();
            _iresponses = new HoaDonChiTietResponse();
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

        [HttpGet("[Action]")]
        public async Task<IEnumerable<HoaDonChiTiet>> GetAllHoaChiTietByMaHD(string maHD)
        {
            return await _iresponse.GetAllItemHoaDon(maHD);
        }

        [HttpPost("[Action]")]
        public async Task<bool> CreateHoaDonCT(string? sku, string? maHD, string? tenSP, decimal? donGia, int? soluong)
        {
            return await _iresponses.CreateHoaDonChiTiet(sku,  maHD, tenSP, donGia, soluong);
        }

        [HttpPost("[Action]")]
        public async Task<bool> TaoHoaDonCT(HoaDonChiTiet hdct)
        {
            HoaDonChiTiet newHDCT = new HoaDonChiTiet
            {
                MaHoaDonChiTiet = Guid.NewGuid().ToString(),
                MaHoaDon = hdct.MaHoaDon,
                SKU = hdct.SKU,
                TenSp = hdct.TenSp,
                SoLuongMua = hdct.SoLuongMua,
                DonGia = hdct.DonGia,
                TrangThai = hdct.TrangThai,
            };
            return await _iresponses.CreateHoaDonChiTiet2(newHDCT);
        }
    }
}
