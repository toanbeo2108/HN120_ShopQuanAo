using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class LoginServices : ILoginServices
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public LoginServices(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<Response> LoginAsync(LoginUser loginUser)
        {
            // Check if the user exists
            var user = await _userManager.Users.FirstOrDefaultAsync(p => p.PhoneNumber == loginUser.PhoneNumber);
            if (user == null)
            {
                return new Response()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Account not found"
                };
            }

            // Check account status
            if (user.Status == 0)
            {
                return new Response()
                {
                    IsSuccess = false,
                    StatusCode = 403,
                    Message = "Your account is locked"
                };
            }
            if (user.Status == -1)
            {
                return new Response()
                {
                    IsSuccess = false,
                    StatusCode = 403,
                    Message = "Unconfirmed account"
                };
            }
            if (!await _userManager.CheckPasswordAsync(user, loginUser.Password))
            {
                return new Response()
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Incorrect password"
                };
            }

            if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
            {
                // Get list of user roles
                var roles = await _userManager.GetRolesAsync(user);

                // Create list of claims
                var claims = new List<Claim>();

                if (!string.IsNullOrEmpty(user.UserName))
                {
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                }
                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    claims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
                }
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                // Set default avatar if it's null
                var avatar = string.IsNullOrEmpty(user.Avatar) ? "AvatarDefault" : user.Avatar;
                claims.Add(new Claim("Avatar", avatar));


                foreach (var userrole in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userrole));
                }

                // Create JWT Token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["JWT:Issuer"]
                    , _configuration["JWT:Audience"], claims, expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: signIn);

                return new Response()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Valid user",
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };

            }
            else
            {
                return new Response()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Invalid user"
                };
            }
        }
    }
}
