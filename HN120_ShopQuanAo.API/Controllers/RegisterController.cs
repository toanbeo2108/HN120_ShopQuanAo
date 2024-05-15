using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : ControllerBase
	{
		private readonly IRegisterServices _registerService;
		public RegisterController(IRegisterServices registerService)
		{
			_registerService = registerService;
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterUser registerUser, string role)
		{
			var result = await _registerService.RegisterAsync(registerUser, role);
			return StatusCode(result.StatusCode, result.Message);
		}
	}
}
