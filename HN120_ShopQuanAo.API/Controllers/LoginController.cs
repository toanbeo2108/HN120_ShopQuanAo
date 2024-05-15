using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly ILoginServices _loginServices;
		public LoginController(ILoginServices loginServices)
		{
			_loginServices = loginServices;
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginUser loginUser)
		{
			var result = await _loginServices.LoginAsync(loginUser);
			if (result.IsSuccess)
			{
				return Ok(result.Token);
			}
			return StatusCode(result.StatusCode, result.Message);
		}
	}
}
