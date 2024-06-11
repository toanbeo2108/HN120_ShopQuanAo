using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HN120_ShopQuanAo.API.Responsitories
{
	public class RegisterServices : IRegisterServices
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public RegisterServices(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<Response> RegisterAsync(RegisterUser registerUser, string role)
		{
			// Kiểm tra xem User đã tồn tại hay chưa
			if (await _userManager.FindByEmailAsync(registerUser.Email) != null)
			{
				return new Response
				{
					IsSuccess = false,
					StatusCode = 400,
					Message = "Email này đã tồn tại!"
				};
			}
			else if (await _userManager.FindByNameAsync(registerUser.PhoneNumber) != null)
			{
				return new Response
				{
					IsSuccess = false,
					StatusCode = 400,
					Message = "Số điện thoại này đã tồn tại!"
				};
			}

			// Kiểm tra Confirm password có đúng so với Password không
			if (registerUser.Password != registerUser.ConfirmPassword)
			{
				return new Response
				{
					IsSuccess = false,
					StatusCode = 400,
					Message = "Mật khẩu và xác nhận mật khẩu không khớp!"
				};
			}

			// Nếu 2 điều kiện bên trên thành công thì tạo ra User mới
			User NewUser = new()
			{
				UserName = registerUser.Username,
				Email = registerUser.Email,
				Status = 1,
				PhoneNumber = registerUser.PhoneNumber
			};

			// Kiểm tra xem phân quyền đã tồn tại hay chưa
			if (await _roleManager.RoleExistsAsync(role))
			{
				// Thêm User vào Database
				var result = await _userManager.CreateAsync(NewUser, registerUser.Password);

				// Nếu đăng ký không thành công
				if (!result.Succeeded)
				{
					return new Response
					{
						IsSuccess = false,
						StatusCode = 500,
						Message = "Đăng ký thất bại, lỗi chưa xác định!"
					};
				}

				// Add role to the user
				await _userManager.AddToRoleAsync(NewUser, role);
				return new Response
				{
					IsSuccess = true,
					StatusCode = 201,
					Message = "Đăng ký thành công!"
				};
			}
			else
			{
				return new Response
				{
					IsSuccess = false,
					StatusCode = 404,
					Message = "Role chưa tồn tại!"
				};
			}
		}
	}
}
