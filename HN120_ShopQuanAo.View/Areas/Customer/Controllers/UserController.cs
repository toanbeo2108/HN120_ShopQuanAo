using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using HN120_ShopQuanAo.Data.ViewModels;

namespace HN120_ShopQuanAo.View.Areas.Customer.Controllers
{
    public class UserController : Controller
    {
        private HttpClient _httpClient;
        public UserController()
        {
            _httpClient = new HttpClient();
        }
        // Cập nhật người dùng

        [HttpGet]
        public async Task<IActionResult> Update(string id, string returnUrl)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Lưu trữ thông tin về trang nguồn vào TempData
            TempData["ReturnUrl"] = returnUrl;

            // Lấy thông tin người dùng
            var userUrl = $"https://localhost:7197/api/User/GetUserById?id={id}";
            var userResponse = await _httpClient.GetAsync(userUrl);
            if (!userResponse.IsSuccessStatusCode)
            {
                return BadRequest("Không thể lấy thông tin người dùng");
            }
            string userData = await userResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(userData);

            // Lấy danh sách địa chỉ người dùng
            var addressUrl = $"https://localhost:7197/api/UserAddress/GetByUserID?id={id}";
            var addressResponse = await _httpClient.GetAsync(addressUrl);
            if (!addressResponse.IsSuccessStatusCode)
            {
                return BadRequest("Không thể lấy danh sách địa chỉ người dùng");
            }
            string addressData = await addressResponse.Content.ReadAsStringAsync();
            var userAddresses = JsonConvert.DeserializeObject<List<DeliveryAddress>>(addressData);
            ViewBag.UserAddress = userAddresses;

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user, IFormFile imageFile)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userUrl = $"https://localhost:7197/api/User/GetUserById?id={user.Id}";
            var userResponse = await _httpClient.GetAsync(userUrl);
            if (!userResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Không thể lấy thông tin người dùng";
                return RedirectToAction("Update", new { id = user.Id });
            }

            string userData = await userResponse.Content.ReadAsStringAsync();
            var ExsitUser = JsonConvert.DeserializeObject<User>(userData);

            if (user.Birthday == null)
            {
                user.Birthday = ExsitUser.Birthday;
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Avatar", imageFile.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                ExsitUser.Avatar = imageFile.FileName;
                ExsitUser.Gender = user.Gender;
                ExsitUser.Birthday = user.Birthday;
                ExsitUser.FullName = user.FullName;
                ExsitUser.PhoneNumber = user.PhoneNumber;
                ExsitUser.Email = user.Email;
            }
            else
            {
                ExsitUser.Gender = user.Gender;
                ExsitUser.Birthday = user.Birthday;
                ExsitUser.FullName = user.FullName;
                ExsitUser.PhoneNumber = user.PhoneNumber;
                ExsitUser.Email = user.Email;
            }

            string apiURL = "https://localhost:7197/api/user/UpdateUser";
            var content = new StringContent(JsonConvert.SerializeObject(ExsitUser), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiURL, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Update", new { id = user.Id });
            }
            else
            {
                string responseMessage = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Lỗi cập nhật người dùng: {responseMessage}";
                return RedirectToAction("Update", new { id = user.Id });
            }
        }


        // Thêm địa chỉ mới

        [HttpGet]
        public async Task<IActionResult> AddUserAddress(string id)
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> AddUserAddress(string id, DeliveryAddressModel address)
        {
            if (!ModelState.IsValid)
            {
                return View(address);
            }


            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            address.UserID = id;
            var url = $"https://localhost:7197/api/UserAddress/Create";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Update", new { id = address.UserID });
            }

            ModelState.AddModelError(string.Empty, "Lỗi thêm địa chỉ.");
            return View(address);
        }


        // Sửa địa chỉ

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
            if (!ModelState.IsValid)
            {
                return View(address);
            }

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var ExistAddressUrl = $"https://localhost:7197/api/UserAddress/GetByID?id=" + id;
            var ExistAddressresponse = await _httpClient.GetAsync(ExistAddressUrl);
            string ExistAddressData = await ExistAddressresponse.Content.ReadAsStringAsync();
            var ExistAddress = JsonConvert.DeserializeObject<DeliveryAddressModel>(ExistAddressData);


            var url = $"https://localhost:7197/api/UserAddress/Update/" + id;
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");
            var respone = await httpClient.PutAsync(url, content);
            if (respone.IsSuccessStatusCode)
            {
                return RedirectToAction("Update", new { id = ExistAddress.UserID });
            }

            ModelState.AddModelError(string.Empty, "Lỗi cập nhật địa chỉ.");
            return View(address);
        }


        // Xóa địa chỉ

        [HttpPost]
        public async Task<IActionResult> DeleteAddressUser(string id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"https://localhost:7197/api/UserAddress/Delete/" + id;
            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Lỗi xóa địa chỉ" });
        }

        // Set địa chỉ mặc định
        [HttpPost]
        public async Task<IActionResult> SetasDefault(string id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"https://localhost:7197/api/UserAddress/SetDefaultAddress/?id=" + id;
            var response = await _httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Lỗi cài đặt địa chỉ mặc định" });
        }

    }
}
