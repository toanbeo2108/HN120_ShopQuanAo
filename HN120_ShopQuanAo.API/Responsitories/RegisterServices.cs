using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Text;

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
        private const int MaxFullNameLength = 50;
        private async Task<string> GenerateUniqueUserName(string fullName)
        {
            // Loại bỏ dấu cách và chuyển thành chữ thường
            string baseUserName = RemoveDiacritics(fullName.Replace(" ", "").ToLower());

            // Kiểm tra tính duy nhất của UserName
            string userName = baseUserName;
            while (await _userManager.FindByNameAsync(userName) != null)
            {
                string suffix = GenerateRandomSuffix();
                userName = baseUserName + suffix;
            }

            return userName;
        }

        // Hàm để loại bỏ dấu và ký tự đặc biệt
        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        // Hàm để tạo ra một hậu tố ngẫu nhiên
        private string GenerateRandomSuffix()
        {
            var random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<Response> RegisterAsync(RegisterUser registerUser, string role)
		{
            // Giới hạn độ dài của FullName
            if (registerUser.FullName.Length > MaxFullNameLength)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Họ và tên không được dài quá {MaxFullNameLength} ký tự."
                };
            }

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
            // Tạo UserName từ FullName và đảm bảo là duy nhất
            string userName = await GenerateUniqueUserName(registerUser.FullName);

            // Nếu 2 điều kiện bên trên thành công thì tạo ra User mới
            User NewUser = new()
			{
				UserName = userName,
				FullName = registerUser.FullName,
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

        public async Task<Response> CreateAnAccount(NewAccountModel NewAccountModel, string role)
        {
            // Giới hạn độ dài của FullName
            if (NewAccountModel.FullName.Length > MaxFullNameLength)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Họ và tên không được dài quá {MaxFullNameLength} ký tự."
                };
            }

            // Kiểm tra xem User đã tồn tại hay chưa
            if (await _userManager.FindByEmailAsync(NewAccountModel.Email) != null)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Email này đã tồn tại!"
                };
            }
            else if (await _userManager.FindByNameAsync(NewAccountModel.PhoneNumber) != null)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Số điện thoại này đã tồn tại!"
                };
            }

            // Kiểm tra Confirm password có đúng so với Password không
            if (NewAccountModel.Password != NewAccountModel.ConfirmPassword)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Mật khẩu và xác nhận mật khẩu không khớp!"
                };
            }
            // Tạo UserName từ FullName và đảm bảo là duy nhất
            string userName = await GenerateUniqueUserName(NewAccountModel.FullName);

            // Nếu 2 điều kiện bên trên thành công thì tạo ra User mới
            User NewUser = new()
            {
                UserName = userName,
                FullName = NewAccountModel.FullName,
                Gender = NewAccountModel.Gender,
                Avatar = NewAccountModel.Avatar,
                Birthday = NewAccountModel.Birthday,
                Email = NewAccountModel.Email,
                Status = 1,
                PhoneNumber = NewAccountModel.PhoneNumber
            };

            // Kiểm tra xem phân quyền đã tồn tại hay chưa
            if (await _roleManager.RoleExistsAsync(role))
            {
                // Thêm User vào Database
                var result = await _userManager.CreateAsync(NewUser, NewAccountModel.Password);

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
