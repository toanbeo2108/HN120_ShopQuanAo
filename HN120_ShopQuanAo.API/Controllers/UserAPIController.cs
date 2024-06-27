using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HN120_ShopQuanAo.API.Controllers
{
	[Route("api/User")]
	[ApiController]
	public class UserAPIController : ControllerBase
	{
		public AppDbContext _context = new AppDbContext();
		private readonly UserManager<User> _userManager;
        private readonly IAddressUserReponse _response;
        public UserAPIController(UserManager<User> userManager, IAddressUserReponse reponse)
		{
			_userManager = userManager;
		}

		[HttpGet]
		[Route("GetAllAccount")]
		//[Authorize]
		public async Task<IEnumerable<User>> GetAllAccount()
		{
			return await _userManager.Users.ToListAsync();
		}
        [HttpGet]
        [Route("GetUsersByRole")]
        //[Authorize(Roles = "Admin")]
        public async Task<IEnumerable<User>> getUserByRole(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);
        }


        [HttpGet]
		[Route("GetUserById")]
		//[Authorize]
		public async Task<User> GetUserById(string id)
		{
			return await _userManager.FindByIdAsync(id);
		}


		[HttpPost]
		[Route("ChangePassword")]
		public async Task<bool> ChangePassword(ChangePasswordModel model)
		{
			var user = await _userManager.FindByNameAsync(model.UserName);
			if (user == null)
			{
				return false;
			}
			var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
			if (!result.Succeeded)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		[HttpPut]
		[Route("UpdateUser")]
		public async Task<User> UpdateUser(User model)
		{
			// Tìm người dùng theo ID
			var user = await _userManager.FindByIdAsync(model.Id);

			// Cập nhật thông tin người dùng
			user.UserName = model.UserName;
			user.FullName = model.FullName;
            user.Email = model.Email;
			user.PhoneNumber = model.PhoneNumber;
			user.Avatar = model.Avatar;
			user.Gender = model.Gender;
			user.Birthday = model.Birthday;
            user.Status = model.Status;
			// Cập nhật các thuộc tính khác của User tại đây

			// Lưu thay đổi
			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				return user;
			}
			else
			{
				return null;
			}
		}

        [HttpDelete]
        [Route("DeleteUser")]

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var deliveryAddresses = _context.DeliveryAddress.Where(da => da.UserID == id);
            _context.DeliveryAddress.RemoveRange(deliveryAddresses);

            // Lưu các thay đổi sau khi xóa các bản ghi phụ thuộc
            await _context.SaveChangesAsync();

            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User deleted successfully");
            }
            else
            {
                return BadRequest("Error deleting user");
            }
        }


        [HttpPut]
        [Route("UpdateUserStatus")]
        public async Task<User> UpdateUserStatus(string id, int status)
        {
            // Tìm người dùng theo ID
            var user = await _userManager.FindByIdAsync(id);

			// Cập nhật thông tin người dùng
			user.Status = status;

            // Lưu thay đổi
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
