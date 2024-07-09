    using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using System.Net;

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

        // Lọc tài khoản    
        [HttpGet]
        public async Task<IActionResult> FilterByRole(string roleName)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7197/api/User/GetUsersByRole?roleName={roleName}";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            return View("GetAllAccount", ListUser);
        }
        // Thêm tài khoản mới
        [HttpGet]
        public async Task<IActionResult> CreateAnAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnAccount(RegisterUser registerUser, string role)
        {
            try
            {
                var token = Request.Cookies["Token"];
                // Convert registerUser to JSON
                var registerUserJSON = JsonConvert.SerializeObject(registerUser);

                // Convert to string content
                var stringContent = new StringContent(registerUserJSON, Encoding.UTF8, "application/json");

                // Add role to queryString
                var queryString = $"?role={role}";

                // Send request POST to register API
                var response = await _httpClient.PostAsync($"https://localhost:7197/api/Register{queryString}", stringContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllAccount", "UserManager");
                }
                var errorResponse = await response.Content.ReadAsStringAsync();
                ViewBag.Message = $"Login failed: {errorResponse}";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Lỗi không xác định: {ex.Message}";
                return View();
            }
        }



        // Sửa trạng thái người dùng
        [HttpGet]
        public async Task<IActionResult> UpdateStatusUser(string id, int status)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7197/api/User/UpdateUserStatus?id={id}&status={status}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var respone = await httpClient.PutAsync(url, content);
            return RedirectToAction("GetAllAccount");
        }

        // Cập nhật người dùng

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

            // Lấy thông tin người dùng
            var userUrl = $"https://localhost:7197/api/User/GetUserById?id={user.Id}";
            var userResponse = await _httpClient.GetAsync(userUrl);
            if (!userResponse.IsSuccessStatusCode)
            {
                return BadRequest("Không thể lấy thông tin người dùng");
            }
            string userData = await userResponse.Content.ReadAsStringAsync();
            var ExsitUser = JsonConvert.DeserializeObject<User>(userData);
            if (user.Birthday == null)
            {
                user.Birthday = ExsitUser.Birthday;
            }
            // Xử lý upload file
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

            // Tuần tự hóa đối tượng người dùng
            var content = new StringContent(JsonConvert.SerializeObject(ExsitUser), Encoding.UTF8, "application/json");

            // Gửi yêu cầu PUT tới API
            var response = await _httpClient.PutAsync(apiURL, content);

            // Kiểm tra mã trạng thái phản hồi
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllAccount");
            }
            else
            {
                // Đọc thông báo lỗi từ API
                string responseMessage = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = $"Lỗi cập nhật người dùng: {responseMessage}";

                // Log chi tiết phản hồi để giúp debug
                var errorDetails = new
                {
                    StatusCode = response.StatusCode,
                    ReasonPhrase = response.ReasonPhrase,
                    RequestMessage = response.RequestMessage.ToString(),
                    ResponseContent = responseMessage
                };
                Console.WriteLine(JsonConvert.SerializeObject(errorDetails, Formatting.Indented));

                return BadRequest($"Lỗi cập nhật người dùng: {responseMessage}");
            }
        }


        // Quản lý địa chỉ người dùng


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
