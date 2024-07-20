using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using HN120_ShopQuanAo.API.Data;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class RegisterServices : IRegisterServices
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public RegisterServices(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        private const int MaxFullNameLength = 50;

        private async Task<string> GenerateUniqueUserName(string fullName)
        {
            // Remove spaces, diacritics and convert to lowercase
            string baseUserName = RemoveDiacritics(fullName.Replace(" ", "").ToLower());

            // Ensure the base username is valid
            baseUserName = EnsureValidUserName(baseUserName);

            // Check uniqueness of UserName
            string userName = baseUserName;
            while (await _userManager.FindByNameAsync(userName) != null)
            {
                string suffix = GenerateRandomSuffix();
                userName = baseUserName + suffix;
            }

            return userName;
        }

        // Function to remove diacritics
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

        // Function to generate a random suffix
        private string GenerateRandomSuffix()
        {
            var random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Function to ensure the username is valid
        private string EnsureValidUserName(string userName)
        {
            var stringBuilder = new StringBuilder();
            foreach (char c in userName)
            {
                if (char.IsLetterOrDigit(c) || c == '_' || c == '-')
                {
                    stringBuilder.Append(c);
                }
            }

            // If the username is empty after removing invalid characters, generate a new one
            if (stringBuilder.Length == 0)
            {
                return "user_" + GenerateRandomSuffix();
            }

            return stringBuilder.ToString();
        }

        public async Task<Response> RegisterAsync(RegisterUser registerUser, string role)
        {
            // Limit the length of FullName
            if (registerUser.FullName.Length > MaxFullNameLength)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Full name must not exceed {MaxFullNameLength} characters."
                };
            }

            // Check if User already exists
            if (await _userManager.FindByEmailAsync(registerUser.Email) != null)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Email already exists!"
                };
            }

            // Check if PhoneNumber already exists
            var existingUserWithPhoneNumber = await _userManager.Users
                .AnyAsync(u => u.PhoneNumber == registerUser.PhoneNumber);

            if (existingUserWithPhoneNumber)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Phone number already exists!"
                };
            }

            // Check if ConfirmPassword matches Password
            if (registerUser.Password != registerUser.ConfirmPassword)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Password and confirmation password do not match!"
                };
            }

            // Generate UserName from FullName and ensure uniqueness
            string userName = await GenerateUniqueUserName(registerUser.FullName);

            // If both conditions above are successful, create a new User
            User newUser = new()
            {
                UserName = userName,
                FullName = registerUser.FullName,
                Email = registerUser.Email,
                Status = 1,
                PhoneNumber = registerUser.PhoneNumber
            };

            // Check if role exists
            if (await _roleManager.RoleExistsAsync(role))
            {
                // Add User to the Database
                var result = await _userManager.CreateAsync(newUser, registerUser.Password);

                // If registration fails
                if (!result.Succeeded)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Message = "Registration failed, " + string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                // Add role to the user
                await _userManager.AddToRoleAsync(newUser, role);

                // Create a new GioHang (Shopping Cart) for the user
                var gioHang = new GioHang
                {
                    MaGioHang = Guid.NewGuid().ToString(),
                    TongTien = 0,
                    MoTa = null,
                    User = newUser,
                    TrangThai = 1
                };

                // Add gioHang to DbContext and save changes
                _context.GioHang.Add(gioHang);
                await _context.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    Message = "Registration successful!"
                };
            }
            else
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Role does not exist!"
                };
            }
        }

        public async Task<Response> CreateAnAccount(NewAccountModel newAccountModel, string role)
        {
            // Limit the length of FullName
            if (newAccountModel.FullName.Length > MaxFullNameLength)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Full name must not exceed {MaxFullNameLength} characters."
                };
            }

            // Check if User already exists
            if (await _userManager.FindByEmailAsync(newAccountModel.Email) != null)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Email already exists!"
                };
            }
            else if (await _userManager.FindByNameAsync(newAccountModel.PhoneNumber) != null)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Phone number already exists!"
                };
            }

            // Check if ConfirmPassword matches Password
            if (newAccountModel.Password != newAccountModel.ConfirmPassword)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Password and confirmation password do not match!"
                };
            }

            // Generate UserName from FullName and ensure uniqueness
            string userName = await GenerateUniqueUserName(newAccountModel.FullName);

            // If both conditions above are successful, create a new User
            User newUser = new()
            {
                UserName = userName,
                FullName = newAccountModel.FullName,
                Gender = newAccountModel.Gender,
                Avatar = newAccountModel.Avatar,
                Birthday = newAccountModel.Birthday,
                Email = newAccountModel.Email,
                Status = 1,
                PhoneNumber = newAccountModel.PhoneNumber
            };

            // Check if role exists
            if (await _roleManager.RoleExistsAsync(role))
            {
                // Add User to the Database
                var result = await _userManager.CreateAsync(newUser, newAccountModel.Password);

                // If registration fails
                if (!result.Succeeded)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Message = "Registration failed, unknown error!"
                    };
                }

                // Add role to the user
                await _userManager.AddToRoleAsync(newUser, role);
                return new Response
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    Message = "Registration successful!"
                };
            }
            else
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Role does not exist!"
                };
            }
        }
    }
}
