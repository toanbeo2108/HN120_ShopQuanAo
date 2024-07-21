using HN120_ShopQuanAo.API.EmailConfig.Services;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using System.Text;
using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.EmailConfig.ViewModel;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.Extensions.Caching.Memory;

namespace HN120_ShopQuanAo.API.Services
{
    public class RegisterUserEmailService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;

        public RegisterUserEmailService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMemoryCache memoryCache, AppDbContext context, EmailService emailService, LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _emailService = emailService;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
        }

        private const int MaxFullNameLength = 50;
        private const string TempPassword = "Abcd@123";

        public async Task<Response> RegisterAsync(RegisterWithEmail registerUser, string role)
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
            var existingUser = await _userManager.FindByEmailAsync(registerUser.Email);
            if (existingUser != null)
            {
                if (existingUser.Status == -1)
                {
                    // Update existing user information
                    existingUser.FullName = registerUser.FullName;
                    existingUser.PhoneNumber = registerUser.PhoneNumber;

                    var updateResult = await _userManager.UpdateAsync(existingUser);
                    if (!updateResult.Succeeded)
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            StatusCode = 500,
                            Message = "Failed to update user information."
                        };
                    }

                    // Generate email confirmation token
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(existingUser);
                    var confirmationLink = _linkGenerator.GetUriByAction(
                        _httpContextAccessor.HttpContext,
                        action: "ConfirmEmail",
                        controller: "Account",
                        values: new { token, email = existingUser.Email },
                        scheme: "https");

                    // Send confirmation email
                    await _emailService.SendEmailAsync(existingUser.Email, "Xác nhận email", $"Vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết sau: {confirmationLink}");

                    return new Response
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = "Please confirm your email to complete the registration."
                    };
                }
                else
                {
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Message = "Email already exists!"
                    };
                }
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

            // Generate UserName from FullName and ensure uniqueness
            string userName = await GenerateUniqueUserName(registerUser.FullName);

            // Generate a random password
            string password = TempPassword;

            // Create a new user
            User newUser = new()
            {
                UserName = userName,
                FullName = registerUser.FullName,
                Email = registerUser.Email,
                Status = -1, // Status -1 indicates not confirmed
                PhoneNumber = registerUser.PhoneNumber
            };

            // Check if role exists
            if (await _roleManager.RoleExistsAsync(role))
            {
                // Add User to the Database
                var result = await _userManager.CreateAsync(newUser, password);
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
                };

                // Add gioHang to DbContext and save changes
                _context.GioHang.Add(gioHang);
                await _context.SaveChangesAsync();

                // Generate email confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var confirmationLink = _linkGenerator.GetUriByAction(
                    _httpContextAccessor.HttpContext,
                    action: "ConfirmEmail",
                    controller: "Account",
                    values: new { token, email = newUser.Email },
                    scheme: "https");

                // Send confirmation email with account details
                await _emailService.SendEmailAsync(newUser.Email, "Xác nhận email", $"Vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết sau: {confirmationLink}");

                // Set a timer to delete the user and their cart if not confirmed within 1 hour
                var timer = new Timer(async (state) =>
                {
                    var user = await _userManager.FindByIdAsync(newUser.Id);
                    if (user != null && user.Status == -1)
                    {
                        // Delete the user's cart
                        var cart = await _context.GioHang.FirstOrDefaultAsync(g => g.User.Id == user.Id);
                        if (cart != null)
                        {
                            _context.GioHang.Remove(cart);
                        }

                        // Delete the user
                        await _userManager.DeleteAsync(user);
                        await _context.SaveChangesAsync();
                    }
                }, null, TimeSpan.FromHours(1), Timeout.InfiniteTimeSpan);

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Please confirm your email to complete the registration."
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

        public async Task<Response> ConfirmEmail(string token, string email)
        {
            // Log token and email
            Console.WriteLine($"Token: {token}, Email: {email}");
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return new Response { IsSuccess = false, StatusCode = 400, Message = "Invalid email confirmation request." };
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new Response { IsSuccess = false, StatusCode = 400, Message = "Invalid email confirmation request." };
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return new Response { IsSuccess = false, StatusCode = 400, Message = "Email confirmation failed." };
            }

            // Update user status to confirmed
            user.Status = 1; // Status 1 indicates confirmed
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return new Response { IsSuccess = false, StatusCode = 500, Message = "Failed to update user status." };
            }

            // Send email with account details
            await _emailService.SendEmailAsync(user.Email, "Tài khoản của bạn đã được xác nhận", $"Tên đăng nhập: {user.PhoneNumber}\nMật khẩu: {TempPassword}" +
                $"\n\n\n Hoặc bạn cũng có thể đăng nhập bằng tài khoản Email của mình : {user.Email} Mật khẩu: {TempPassword}");

            return new Response { IsSuccess = true, StatusCode = 200, Message = "Email confirmed and account activated successfully!" };
        }
        public async Task<Response> DeleteUserAndCartByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "User not found."
                };
            }

            // Find the user's cart
            var cart = await _context.GioHang.FirstOrDefaultAsync(g => g.User.Id == userId);
            if (cart != null)
            {
                _context.GioHang.Remove(cart);
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "Failed to delete user."
                };
            }

            await _context.SaveChangesAsync();

            return new Response
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "User and cart deleted successfully."
            };
        }

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

        private string GenerateRandomSuffix()
        {
            var random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 3).Select(s => s[random.Next(s.Length)]).ToArray());
        }

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
    }
}
