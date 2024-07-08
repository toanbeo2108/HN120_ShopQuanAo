using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAddressUserReponse _response;

        public UserAddressController(AppDbContext context, IAddressUserReponse reponse)
        {
            _context = context;
            _response = reponse;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _response.GetAll();
            if (list == null || !list.Any())
            {
                return NoContent();
            }
            return Ok(list);
        }
        [HttpGet("GetByID")]
        public async Task<IActionResult> GetByID(string id)
        {
            var dc = await _response.GetByID(id);
            return Ok(dc);
        }
        [HttpGet("GetByUserID")]
        public async Task<IActionResult> GetByUserID(string id)
        {
            var alldc = await _response.GetAll();
            var list = alldc.Where(dc => dc.UserID == id);
            if (list == null || !list.Any())
            {
                return NoContent();
            }
            return Ok(list);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(DeliveryAddressModel item)
        {
            var result = await _response.Create(item);
            if (result.Success)
            {
                return Ok(new { message = "Tạo địa chỉ thành công." });
            }
            else
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, DeliveryAddressModel item)
        {
            var result = await _response.Update(id, item);
            if (result.Success)
            {
                return Ok(new { message = "Cập nhật địa chỉ thành công." });
            }
            else
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _response.Delete(id);
            if (result.Success)
            {
                return Ok(new { message = "Xóa địa chỉ thành công." });
            }
            else
            {
                return BadRequest(new { error = result.ErrorMessage });
            }
        }


        [HttpPost("SetDefaultAddress")]
        public async Task<IActionResult> SetDefaultAddress(string id)
        {
            var result = await _response.SetasDefault(id);

            return Ok(result.ErrorMessage);
        }
    }
}
