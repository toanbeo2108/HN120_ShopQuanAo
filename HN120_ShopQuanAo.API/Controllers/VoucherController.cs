using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{

    public class VoucherController : Controller
    {
        private readonly IVoucherServices _sv;
        public VoucherController(IVoucherServices svi)
        {
            _sv = svi;
        }
        [HttpGet("[Action]")]
        public IActionResult GetAllVoucher()
        {
            var vc = _sv.GetAllVoucher();
            return Ok(vc);
        }

        [HttpGet("[Action]/{ma}")]
        public IActionResult GetByVCMa(string ma)
        {
            try
            {
                var vc = _sv.GetVoucherByMa(ma);
                return Ok(vc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("[Action]")]
        public IActionResult CreateVCher(Voucher vc)
        {
            try
            {
                _sv.CreateVoucher(vc);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("[Action]")]
        public IActionResult UpdateVCher(Voucher vc)
        {
            try
            {
                _sv.UpdateVoucher(vc);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("[Action]/{ma}")]
        public IActionResult DeleteVcher(string ma)
        {
            try
            {
                _sv.DeleteVoucher(ma);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
