using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
			// Check user is exists
			var user = await _userManager.Users.FirstOrDefaultAsync(p => p.PhoneNumber == loginUser.PhoneNumber);
			if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
			{
				// Get list roles of user
				var roles = await _userManager.GetRolesAsync(user);

				// Create list of claims
				var claims = new List<Claim>()
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
				};
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
