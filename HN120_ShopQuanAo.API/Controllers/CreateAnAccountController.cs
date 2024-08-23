using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateAnAccountController : ControllerBase
    {
        private readonly ICreateAnAccount _createAnAccount;
        public CreateAnAccountController(ICreateAnAccount createAnAccount)
        {
            _createAnAccount = createAnAccount;
        }

        [HttpPost]
        public async Task<IActionResult> AdminCreateAnAccount(NewAccountModel newAccountModel, string role, string? userId = null)
        {
            var result = await _createAnAccount.AdminCreateAccount(newAccountModel, role, userId);
            return StatusCode(result.StatusCode, result.Message);
        }

    }
}
