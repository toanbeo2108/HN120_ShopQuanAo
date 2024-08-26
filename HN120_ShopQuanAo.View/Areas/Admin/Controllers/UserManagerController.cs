using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using HN120_ShopQuanAo.View.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
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
        public async Task<IActionResult> GetAllUser()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7197/api/User/GetUsersByRole?roleName=User";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);


            // Truyền thông tin về trang nguồn vào View
            ViewBag.ReturnUrl = Url.Action("GetAllUser", "UserManager").ToString();
            return View(ListUser);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmpolyee()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7197/api/User/GetUsersByRole?roleName=Employee";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);

            // Truyền thông tin về trang nguồn vào View
            ViewBag.ReturnUrl = Url.Action("GetAllUser", "UserManager");
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
            var check = ViewBag.ReturnUrl;
            return View(check);
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
                    var check = ViewBag.ReturnUrl;
                    return RedirectToAction(check);
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

        [HttpGet]
        public async Task<IActionResult> CreateUserAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserAccount(CreateUserAccountViewModel model, IFormFile imageFile)
        {
            try
            {
                // Tạo  UserID cho NewAccount
                var userId = Guid.NewGuid().ToString();
                model.DeliveryAddress.UserID = userId;
                model.DeliveryAddress.Status = 1;

                // Xử lý ảnh đại diện (nếu có)
                if (imageFile != null && imageFile.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Avatar", imageFile.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    model.NewAccount.Avatar = imageFile.FileName;
                }

                //Thêm tài khoản mới:
                var url1 = $"https://localhost:7197/api/CreateAnAccount?role=User&userId={userId}";
                var httpClient1 = new HttpClient();
                var content1 = new StringContent(JsonConvert.SerializeObject(model.NewAccount), Encoding.UTF8, "application/json");
                var response1 = await httpClient1.PostAsync(url1, content1);
                if (!response1.IsSuccessStatusCode)
                {
                    // Đọc nội dung phản hồi từ API
                    var errorContent = await response1.Content.ReadAsStringAsync();

                    // Ghi log hoặc trả về lỗi chi tiết
                    if (response1.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound($"API không tìm thấy. URL: {url1}. Nội dung lỗi: {errorContent}");
                    }

                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo tài khoản: " + errorContent;
                    return View(model);
                }


                var url = $"https://localhost:7197/api/UserAddress/Create";
                var httpClient = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(model.DeliveryAddress), Encoding.UTF8, "application/json");
                var response2 = await httpClient.PostAsync(url, content);
                if (!response2.IsSuccessStatusCode)
                {
                    // Đọc nội dung phản hồi từ API
                    var errorContent2 = await response2.Content.ReadAsStringAsync();

                    // Ghi log hoặc trả về lỗi chi tiết
                    if (response2.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound($"API không tìm thấy. URL: {url1}. Nội dung lỗi: {errorContent2}");
                    }

                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo tài khoản: " + errorContent2;
                    return View(model);
                }
                return RedirectToAction("GetAllUser");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo tài khoản: " + ex.Message;
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> CreateEmployeeAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAccount(CreateUserAccountViewModel model, IFormFile imageFile)
        {
            try
            {
                // Tạo  UserID cho NewAccount
                var userId = Guid.NewGuid().ToString();
                model.DeliveryAddress.UserID = userId;
                model.DeliveryAddress.Status = 1;

                // Xử lý ảnh đại diện (nếu có)
                if (imageFile != null && imageFile.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Avatar", imageFile.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    model.NewAccount.Avatar = imageFile.FileName;
                }

                //Thêm tài khoản mới:
                var url1 = $"https://localhost:7197/api/CreateAnAccount?role=Employee&userId={userId}";
                var httpClient1 = new HttpClient();
                var content1 = new StringContent(JsonConvert.SerializeObject(model.NewAccount), Encoding.UTF8, "application/json");
                var response1 = await httpClient1.PostAsync(url1, content1);
                if (!response1.IsSuccessStatusCode)
                {
                    // Đọc nội dung phản hồi từ API
                    var errorContent = await response1.Content.ReadAsStringAsync();

                    // Ghi log hoặc trả về lỗi chi tiết
                    if (response1.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound($"API không tìm thấy. URL: {url1}. Nội dung lỗi: {errorContent}");
                    }

                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo tài khoản: " + errorContent;
                    return View(model);
                }


                var url = $"https://localhost:7197/api/UserAddress/Create";
                var httpClient = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(model.DeliveryAddress), Encoding.UTF8, "application/json");
                var response2 = await httpClient.PostAsync(url, content);
                if (!response2.IsSuccessStatusCode)
                {
                    // Đọc nội dung phản hồi từ API
                    var errorContent2 = await response2.Content.ReadAsStringAsync();

                    // Ghi log hoặc trả về lỗi chi tiết
                    if (response2.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound($"API không tìm thấy. URL: {url1}. Nội dung lỗi: {errorContent2}");
                    }

                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo tài khoản: " + errorContent2;
                    return View(model);
                }
                return RedirectToAction("GetAllEmployee");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo tài khoản: " + ex.Message;
                return View(model);
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

            return RedirectToAction("GetAllUser");
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
                var returnUrl = TempData["ReturnUrl"]?.ToString();
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("GetAllUser");
            }
            else
            {
                string responseMessage = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Lỗi cập nhật người dùng: {responseMessage}";
                return RedirectToAction("Update", new { id = user.Id });
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
