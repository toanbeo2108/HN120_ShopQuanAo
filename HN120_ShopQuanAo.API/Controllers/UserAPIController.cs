using HN120_ShopQuanAo.API.Data;
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
		public UserAPIController(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		[HttpGet]
		[Route("GetAllUser")]
		//[Authorize]
		public async Task<IEnumerable<User>> GetAllUser()
		{
			return await _userManager.Users.ToListAsync();
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
		//[Authorize]
		public async Task<User> UpdateUser(User model)
		{
			// Tìm người dùng theo ID
			var user = await _userManager.FindByIdAsync(model.Id);

			// Cập nhật thông tin người dùng
			user.UserName = model.UserName;
			user.Email = model.Email;
			user.PhoneNumber = model.PhoneNumber;
			user.Avatar = model.Avatar;
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




	}
}
