using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class KhuyenMaiController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<KhuyenMaiController> _logger;

        public KhuyenMaiController(ILogger<KhuyenMaiController> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllKhuyenMaiManager(string searchString, int? searchStatus)
        {
            var urlBook = $"https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDataBook);
            if (!string.IsNullOrEmpty(searchString))
            {
                lstBook = lstBook.Where(x => x.TenKhuyenMai.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (searchStatus.HasValue)
            {
                lstBook = lstBook.Where(x => x.TrangThai == searchStatus.Value).ToList();
            }
            ViewData["CurrentFilter"] = searchString;
            return View(lstBook);
        }

        [HttpGet]
        public IActionResult CreateKhuyenMai()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateKhuyenMai(KhuyenMai bk)
        {
            //var urlBook = $"https://localhost:7197/api/KhuyenMai/AddKM?TenKhuyenMai={bk.TenKhuyenMai}&PhanTramGiam={bk.PhanTramGiam}";
            //var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            //var respon = await _httpClient.PostAsync(urlBook, content);
            //if (respon.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("AllKhuyenMaiManager", "KhuyenMai", new { area = "Admin" });
            //}
            //TempData["errorMessage"] = "Thêm thất bại";
            //return View();
            if (await IsDuplicateKhuyenMai(bk.TenKhuyenMai))
            {
                TempData["errorMessage"] = "Tên đã tồn tại.";
                return View();
            }

            var urlBook = $"https://localhost:7197/api/KhuyenMai/AddKM";
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllKhuyenMaiManager", "KhuyenMai", new { area = "Admin" });
            }
            TempData["errorMessage"] = "Thêm thất bại";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> KhuyenMaiDetail(string id)
        {
            var urlBook = $"https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaKhuyenMai == id);
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
        public async Task<IActionResult> UpdateKhuyenMai(string id)
        {
            var urlBook = $"https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaKhuyenMai == id);
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
        public async Task<IActionResult> UpdateKhuyenMai(string id, KhuyenMai bk)
        {
            if (await IsDuplicateKhuyenMai(bk.TenKhuyenMai))
            {
                TempData["errorMessage"] = "Tên đã tồn tại.";
                return View();
            }
            var urlBook = $"https://localhost:7197/api/KhuyenMai/UpdateKM/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            return RedirectToAction("AllKhuyenMaiManager", "KhuyenMai", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusKhuyenMaiKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/KhuyenMai/UpdateStatusKhuyenMai/{id}?_ctsp=1";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Kinh Doanh: {Error}", errorMessage);
                return BadRequest($"Failed to update status to Kinh Doanh. Error: {errorMessage}");
            }
            return RedirectToAction("AllKhuyenMaiManager");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusKhuyenMaiKKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/KhuyenMai/UpdateStatusKhuyenMai/{id}?_ctsp=0";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Không Kinh Doanh: {Error}", errorMessage);
                return BadRequest($"Failed to update status to Không Kinh Doanh. Error: {errorMessage}");
            }
            return RedirectToAction("AllKhuyenMaiManager");
        }
        private async Task<bool> IsDuplicateKhuyenMai(string tenKhuyenMai, string id = null)
        {
            var urlBook = $"https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDataBook);

            if (id == null)
            {
                return lstBook.Any(x => x.TenKhuyenMai == tenKhuyenMai);
            }
            else
            {
                return lstBook.Any(x => x.TenKhuyenMai == tenKhuyenMai && x.MaKhuyenMai != id);
            }
        }
    }
}