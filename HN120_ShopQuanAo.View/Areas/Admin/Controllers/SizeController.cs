using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class SizeController : Controller
    {
        private HttpClient _httpClient;
        private readonly ILogger<SizeController> _logger;

        public SizeController(ILogger<SizeController> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;

        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7197/api/Size/GetAllSize
        //https://localhost:7197/api/Size/add-SZ?Tensz=1&MoTa=1&TrangThai=1
        //    https://localhost:7197/api/Size/update-SZ
        [HttpGet]
        public async Task<IActionResult> AllSizeManager(string searchString)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/Size/GetAllSize";
            //var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Size>>(apiDataBook);
            if (!string.IsNullOrEmpty(searchString))
            {
                lstBook = lstBook.Where(x => x.TenSize.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewData["CurrentFilter"] = searchString;
            return View(lstBook);
        }
        [HttpGet]
        public IActionResult CreateSize()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSize(Size bk)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //bk.CreateDate = DateTime.Now;
            if (await IsDuplicateSize(bk.TenSize))
            {
                TempData["errorMessage"] = "Tên đã tồn tại.";
                return View();
            }
            var urlBook = $"https://localhost:7197/api/Size/AddSZ?Tensz={bk.TenSize}&MoTa={bk.MoTa}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllSizeManager", "Size", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SizeDetail(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/Size/GetAllSize";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Size>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaSize == id);
            if (Book == null)
            {
                return BadRequest();
            }
            else
            {
                return View(Book);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateSize(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/Size/GetAllSize";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Size>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaSize == id);
            if (Book == null)
            {
                return BadRequest();
            }
            else
            {
                return View(Book);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSize(string id, Size vc)
        {
            if (await IsDuplicateSize(vc.TenSize, id))
            {
                TempData["errorMessage"] = "Tên đã tồn tại.";
                return View();
            }
            var urlBook = $"https://localhost:7197/api/Size/UpdateSZ/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllSizeManager", "Size", new { area = "Admin" });

        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatusSizeKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/Size/UpdateStatusSize/{id}?_ctsp=1";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Kinh Doanh: {Error}", errorMessage);
                return BadRequest($"Failed to update status to Kinh Doanh. Error: {errorMessage}");
            }
            return RedirectToAction("AllSizeManager");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusSizeKKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/Size/UpdateStatusSize/{id}?_ctsp=0";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Không Kinh Doanh: {Error}", errorMessage);
                return BadRequest($"Failed to update status to Không Kinh Doanh. Error: {errorMessage}");
            }
            return RedirectToAction("AllSizeManager");
        }
        private async Task<bool> IsDuplicateSize(string tenMauSac, string id = null)
        {
            var urlBook = $"https://localhost:7197/api/Size/GetAllSize";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Size>>(apiDataBook);

            if (id == null)
            {
                return lstBook.Any(x => x.TenSize == tenMauSac);
            }
            else
            {
                return lstBook.Any(x => x.TenSize == tenMauSac && x.MaSize != id);
            }
        }
    }
}
