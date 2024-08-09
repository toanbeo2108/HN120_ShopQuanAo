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
using HN120_ShopQuanAo.API.EmailConfig.ViewModel;
using HN120_ShopQuanAo.Data.Models;

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

                _logger.LogInformation($"Response Data: {responseData}");

                var token = responseData.Trim('"');
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
                    Response.Cookies.Append("UserId", nameIdentifierClaim.Value, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddDays(1)
                    });
                }

                var avatarClaim = jwt.Claims.FirstOrDefault(u => u.Type == "Avatar");
                if (avatarClaim != null)
                {
                    identity.AddClaim(new Claim("Avatar", avatarClaim.Value));
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
                else if (roleClaims.Any(rc => rc.Value == "User"))
                {
                    return RedirectToAction("Index", "CustomerHome", new { area = "Customer" });
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

        [HttpGet]
        public IActionResult RegisterEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterEmail(RegisterWithEmail registerUser, string role)
        {
            // Convert registerUser to JSON
            var registerUserJSON = JsonConvert.SerializeObject(registerUser);

            // Convert to string content
            var stringContent = new StringContent(registerUserJSON, Encoding.UTF8, "application/json");

            // Add role to queryString
            role = "User";
            var queryString = $"?role={role}";

            // Send request POST to register API
            var response = await _httpClient.PostAsync($"https://localhost:7197/api/Account/register{queryString}", stringContent);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessageRegister"] = "Đã gửi email!";
                return RedirectToAction("Index", "Home");
            }
            var errorResponse = await response.Content.ReadAsStringAsync();
            ViewBag.Message = $"Register failed: {errorResponse}";
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


        [HttpGet]
        public async Task<IActionResult> Shop(string query)
        {
            var ApiurlSanPham = $"https://localhost:7197/api/SanPham/GetAllSanPham";
            var resposeSP = await _httpClient.GetAsync(ApiurlSanPham);
            string apidatasSP = await resposeSP.Content.ReadAsStringAsync();
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apidatasSP);
            if (!string.IsNullOrEmpty(query))
            {
                lstSP = lstSP.Where(x => x.TrangThai == 1).ToList();
                lstSP = lstSP.Where(x => x.TenSP.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            ViewData["CurrentFilter"] = query;
            return View(lstSP);
        }
        [HttpGet]
        public async Task<IActionResult> DetailSP(string masp)
        {
            // lấy danh sách sản phẩm
            var ApiurlSanPham = $"https://localhost:7197/api/SanPham/GetAllSanPham";
            var resposeSP = await _httpClient.GetAsync(ApiurlSanPham);
            string apidatasSP = await resposeSP.Content.ReadAsStringAsync();
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apidatasSP);
            if (lstSP == null)
            {
                TempData["MessageSPNull"] = "Không lấy được dữ liệu sản phẩm";
                return View();
            }
            // lấy danh sách sản phẩm có trạng thái khả dụng
            var lstSP_ok = lstSP.Where(x => x.TrangThai == 1).ToList();
            if (lstSP_ok == null)
            {
                TempData["MessageSPNull"] = "Không lấy được dữ liệu sản phẩm khả dụng";
                return View();
            }
            // list sản phẩm có thể order thêm ở bên dưới (không quan trọng)
            var lstSP_CanOrder = lstSP_ok.OrderBy(x => x.NgayNhap).Take(5);
            ViewBag.lstSP_CanOrder = lstSP_CanOrder;
            //lấy sản phẩm Detail   
            var SP = lstSP_ok.FirstOrDefault(x => x.MaSp == masp);
            if (SP == null)
            {
                TempData["MessageSPNull"] = "Không lấy được dữ liệu sản phẩm cần mua";
                return View();
            }
            ViewBag.SP = SP;

            // Lấy toàn bộ danh sách CTSP
            var urlSPCT = $"https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
            var responBook = await _httpClient.GetAsync(urlSPCT);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDataBook);
            if (lstBook == null)
            {
                TempData["Error Message"] = "Sản Phẩm không khả dụng";
            }
            //Lấy danh sách sản phẩm chi tiết thông qua mã sản phâm

            var ListCTSP = lstBook.Where(x => x.MaSp == SP.MaSp).ToList();
            if (ListCTSP == null)
            {
                TempData["MessageSPCTNull"] = "Sản Phẩm hiện không khả dụng";
            }
            ViewBag.ListCTSP = ListCTSP;
            var minCTSP = ListCTSP.OrderBy(x => x.GiaBan).FirstOrDefault();
            ViewBag.minCTSP = minCTSP;
            var maxCTSP = ListCTSP.OrderBy(x => x.GiaBan).LastOrDefault();
            ViewBag.maxCTSP = maxCTSP;
            // Lấy List Size
            var urlSize = $"https://localhost:7197/api/Size/GetAllSize";
            //var httpClient = new HttpClient();
            var ResponSize = await _httpClient.GetAsync(urlSize);
            string apidataSize = await ResponSize.Content.ReadAsStringAsync();
            var lstSize = JsonConvert.DeserializeObject<List<Size>>(apidataSize);
            if (lstSize == null)
            {
                TempData["MessageSizeNull"] = "Sản phẩm này đang không có Size";
            }
            ViewBag.ListSize = lstSize;
            // Lấy List Màu Sắc
            var urlMauSac = $"https://localhost:7197/api/MauSac/GetAllMauSac";
            //var httpClient = new HttpClient();
            var responMS = await _httpClient.GetAsync(urlMauSac);
            string apiDataMS = await responMS.Content.ReadAsStringAsync();
            var lstMauSac = JsonConvert.DeserializeObject<List<MauSac>>(apiDataMS);
            if (lstMauSac == null)
            {
                TempData["MessageMauSacNull"] = "Sản phẩm này không có màu";
            }
            ViewBag.lstMauSac = lstMauSac;
            return View();
        }
    }
}