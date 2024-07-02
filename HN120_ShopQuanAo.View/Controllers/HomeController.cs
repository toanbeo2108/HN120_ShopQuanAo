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
            var loginUserJSON = JsonConvert.SerializeObject(loginUser);
            var stringContent = new StringContent(loginUserJSON, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"https://localhost:7197/api/Login", stringContent);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                // In ra phản hồi để kiểm tra
                _logger.LogInformation($"Response Data: {responseData}");

                // Giả sử rằng phản hồi chứa token dưới dạng chuỗi
                var token = responseData.Trim('"');  // Loại bỏ dấu ngoặc kép nếu cần
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
                    // Lưu UserId vào cookie
                    Response.Cookies.Append("UserId", nameIdentifierClaim.Value, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddDays(1)
                    });
                }

                var roleClaims = jwt.Claims.Where(u => u.Type == ClaimTypes.Role).ToList();
                foreach (var roleClaim in roleClaims)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
                }

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                var check = User.Identity.IsAuthenticated;
                if (roleClaims.Any(rc => rc.Value == "Admin"))
                {
                    return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
                }
                else if (roleClaims.Any(rc => rc.Value == "Customer"))
                {
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }

                TempData["SuccessMessage"] = "Đăng nhập thành công!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Message = $"Login failed: {errorResponse}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser, string role)
        {
            // Convert registerUser to JSON
            var registerUserJSON = JsonConvert.SerializeObject(registerUser);

            // Convert to string content
            var stringContent = new StringContent(registerUserJSON, Encoding.UTF8, "application/json");

            // Add role to queryString
            role = "User";
            var queryString = $"?role={role}";

            // Send request POST to register API
            var response = await _httpClient.PostAsync($"https://localhost:7197/api/Register{queryString}", stringContent);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessageRegister"] = "Đăng ký thành công!";
                return RedirectToAction("Index", "Home");
            }
            var errorResponse = await response.Content.ReadAsStringAsync();
            ViewBag.Message = $"Login failed: {errorResponse}";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
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