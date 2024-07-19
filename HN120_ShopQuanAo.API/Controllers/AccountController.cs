using HN120_ShopQuanAo.API.EmailConfig.ViewModel;
using HN120_ShopQuanAo.API.Services;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly RegisterUserEmailService _registerUserEmailService;

        public AccountController(RegisterUserEmailService registerUserEmailService)
        {
            _registerUserEmailService = registerUserEmailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterWithEmail registerUser, string role)
        {
            var response = await _registerUserEmailService.RegisterAsync(registerUser, role);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var response = await _registerUserEmailService.ConfirmEmail(token, email);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var response = await _registerUserEmailService.DeleteUserAndCartByIdAsync(userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
