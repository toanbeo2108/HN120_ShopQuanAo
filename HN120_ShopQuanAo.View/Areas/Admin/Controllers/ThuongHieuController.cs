using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class ThuongHieuController : Controller
    {
        private HttpClient _httpClient;
        private readonly ILogger<ThuongHieuController> _logger;

        public ThuongHieuController(ILogger<ThuongHieuController> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllThuongHieuManager(string searchString)
        {
            var urlBook = $"https://localhost:7197/api/ThuongHieu/GetAllThuongHieu";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ThuongHieu>>(apiDataBook);

            if (!string.IsNullOrEmpty(searchString))
            {
                lstBook = lstBook.Where(x => x.TenThuongHieu.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewData["CurrentFilter"] = searchString;
            return View(lstBook);
        }

        [HttpGet]
        public IActionResult CreateThuongHieu()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateThuongHieu(ThuongHieu bk)
        {
            if (await IsDuplicateThuongHieu(bk.TenThuongHieu))
            {
                ViewData["ErrorMessage"] = "Tên thương hiệu đã tồn tại.";
                return View(bk);
            }

            var urlBook = $"https://localhost:7197/api/ThuongHieu/AddThuongHieu?Tenth={bk.TenThuongHieu}&MoTa={bk.MoTa}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllThuongHieuManager", "ThuongHieu", new { area = "Admin" });
            }
            TempData["ErrorMessage"] = "Thêm thất bại.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateThuongHieu(string id)
        {
            var urlBook = $"https://localhost:7197/api/ThuongHieu/GetAllThuongHieu";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ThuongHieu>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaThuongHieu == id);
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
        public async Task<IActionResult> UpdateThuongHieu(string id, ThuongHieu vc)
        {
            if (await IsDuplicateThuongHieu(vc.TenThuongHieu, id))
            {
                ViewData["ErrorMessage"] = "Tên thương hiệu đã tồn tại.";
                return View(vc);
            }


            var urlBook = $"https://localhost:7197/api/ThuongHieu/EditThuongHieu/{id}";

            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            return RedirectToAction("AllThuongHieuManager", "ThuongHieu", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusThuongHieuKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/ThuongHieu/UpdateStatusThuongHieu/{id}?_ctsp=1";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Kinh Doanh: {Error}", errorMessage);
                return BadRequest($"Failed to update status to Kinh Doanh. Error: {errorMessage}");
            }
            return RedirectToAction("AllThuongHieuManager");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusThuongHieuKKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/ThuongHieu/UpdateStatusThuongHieu/{id}?_ctsp=0";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Không Kinh Doanh: {Error}", errorMessage);
                return BadRequest($"Failed to update status to Không Kinh Doanh. Error: {errorMessage}");
            }
            return RedirectToAction("AllThuongHieuManager");
        }

        private async Task<bool> IsDuplicateThuongHieu(string tenThuongHieu, string id = null)
        {
            var urlBook = $"https://localhost:7197/api/ThuongHieu/GetAllThuongHieu";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ThuongHieu>>(apiDataBook);

            if (id == null)
            {
                return lstBook.Any(x => x.TenThuongHieu == tenThuongHieu);
            }
            else
            {
                return lstBook.Any(x => x.TenThuongHieu == tenThuongHieu && x.MaThuongHieu != id);
            }
        }
    }
}