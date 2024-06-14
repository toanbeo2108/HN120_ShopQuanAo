using HN120_ShopQuanAo.Data.ViewModels;
using HN120_ShopQuanAo.View.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HN120_ShopQuanAo.View.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly HttpClient _httpClient;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
            _httpClient = new HttpClient();
        }

        public IActionResult Index()
		{
			return View();
		}
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginUser loginUser)
		{
			// Convert registerUser to JSON
			var loginUserJSON = JsonConvert.SerializeObject(loginUser);
			// Convert to string content
			var stringContent = new StringContent(loginUserJSON, Encoding.UTF8, "application/json");
			// Send request POST to register API
			var response = await _httpClient.PostAsync($"https://localhost:7197/api/Login", stringContent);
			if (response.IsSuccessStatusCode)
			{
				var token = await response.Content.ReadAsStringAsync();
				var handler = new JwtSecurityTokenHandler();
				var jwt = handler.ReadJwtToken(token);

				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                var nameClaim = jwt.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name);
                if (nameClaim != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Name, nameClaim.Value));
                }

                var nameIdentifierClaim = jwt.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
                if (nameIdentifierClaim != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, nameIdentifierClaim.Value));
                }
				var roleClaim = jwt.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role);
				if (roleClaim != null)
				{
					identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
				}
				var principal = new ClaimsPrincipal(identity);
				await HttpContext.SignInAsync(principal);

				var check = User.Identity.IsAuthenticated;
				// RiRedirect to Area
				if (roleClaim != null)
				{
					if (roleClaim.Value == "Admin")
					{
						return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
					}
					else if (roleClaim.Value == "Customer")
					{
						return RedirectToAction("Index", "Home", new { area = "Customer" });
					}
				}
				// Nếu k thì chuyển về màn hình index mặc định
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ViewBag.Message = await response.Content.ReadAsStringAsync();
				return View();
			}
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}