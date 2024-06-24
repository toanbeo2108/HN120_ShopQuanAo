using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class UserManagerController : Controller
    {
        private HttpClient _httpClient;

        public UserManagerController()
        {
            _httpClient = new HttpClient();
        }

        // Quản lý tất cả tài khoản
        [HttpGet]
        public async Task<IActionResult> GetAllAccount()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7197/api/User/GetAllAccount";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            return View(ListUser);
        }
        
        // Mở form
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7197/api/User/GetUserById?id=" + id;
            var httpClient = new HttpClient();
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var User = JsonConvert.DeserializeObject<User>(apiDataUser);

            var urlDc = $"https://localhost:7197/api/UserAddress/GetByUserID?id=" + id;
            var responseDc = await _httpClient.GetAsync(urlDc);
            string apiDataDc = await responseDc.Content.ReadAsStringAsync();
            var UserAddress = JsonConvert.DeserializeObject<List<DeliveryAddress>>(apiDataDc);
            ViewBag.UserAddress = UserAddress;

            return View(User);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user, IFormFile imageFile)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string apiURL = $"https://localhost:7197/api/user/UpdateUser";

            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Avatar", imageFile.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                user.Avatar = imageFile.FileName;
            }
            else
            {
                var url = $"https://localhost:7197/api/User/GetUserById?id=" + user.Id;
                var responseUrl = await _httpClient.GetAsync(url);
                string apiDataUser = await responseUrl.Content.ReadAsStringAsync();
                var existingUser = JsonConvert.DeserializeObject<User>(apiDataUser);
                user.Avatar = existingUser.Avatar;
            }
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiURL, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllAccount");
            }
            else
            {
                return BadRequest();
            }
        }


        // Quản lý địa chỉ người dùng
        [HttpGet]
        public async Task<IActionResult> AddUserAddress(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAddress(string id,DeliveryAddressModel address)
        {   
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            address.UserID = id;
            var url = $"https://localhost:7197/api/UserAddress/Create";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");
            var respone = await httpClient.PostAsync(url, content);
            if (respone.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllAccount");
            }
            return BadRequest();
        }


        [HttpGet]
        public async Task<IActionResult> UpdateUserAddress(string id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7197/api/UserAddress/GetByID?id=" + id;
            var response = await _httpClient.GetAsync(url);
            string apiData = await response.Content.ReadAsStringAsync();
            var DeliveryAddress = JsonConvert.DeserializeObject<DeliveryAddressModel>(apiData);
            return View(DeliveryAddress);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserAddress(string id, DeliveryAddressModel address)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
           
            var url = $"https://localhost:7197/api/UserAddress/Update/" + id;
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");
            var respone = await httpClient.PutAsync(url, content);
            if (respone.IsSuccessStatusCode)
            {
                return RedirectToAction("Update", new { id = address.UserID });
            }
            return BadRequest("Lỗi");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAddressUser(string id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"https://localhost:7197/api/UserAddress/Delete/" + id;
            var httpClient = new HttpClient();
            var respone = await httpClient.DeleteAsync(url);
            if (respone.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllAccount");
            }
            return BadRequest("Lỗi");
        }

    }
}
