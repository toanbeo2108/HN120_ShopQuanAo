using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class TheLoaiController : Controller
    {
        private HttpClient _httpClient;
        private readonly ILogger<TheLoaiController> _logger;

        public TheLoaiController(ILogger<TheLoaiController> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7197/api/TheLoai/GetAllTheLoai
        //https://localhost:7197/api/TheLoai/add-TL?Tentl=1&MoTa=1&TrangThai=1
        //https://localhost:7197/api/TheLoai/update-TL
        [HttpGet]
        public async Task<IActionResult> AllTheLoaiManager(string searchString)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/TheLoai/GetAllTheLoai";
            //var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataBook);
            if (!string.IsNullOrEmpty(searchString))
            {
                lstBook = lstBook.Where(x => x.TenTheLoai.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewData["CurrentFilter"] = searchString;
            return View(lstBook);
        }
        [HttpGet]
        public IActionResult CreateTheLoai()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTheLoai(TheLoai bk)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (await IsDuplicateTheLoai(bk.TenTheLoai))
            {
                TempData["errorMessage"] = "Tên đã tồn tại.";
                return View();
            }
            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7197/api/TheLoai/AddTL?Tentl={bk.TenTheLoai}&MoTa={bk.MoTa}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllTheLoaiManager", "TheLoai", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> TheLoaiDetail(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/TheLoai/GetAllTheLoai";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaTheLoai == id);
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
        public async Task<IActionResult> UpdateTheLoai(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/TheLoai/GetAllTheLoai";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaTheLoai == id);
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
        public async Task<IActionResult> UpdateTheLoai(string id, TheLoai vc)
        {
            if (await IsDuplicateTheLoai(vc.TenTheLoai,id))
            {
                TempData["errorMessage"] = "Tên đã tồn tại.";
                return View();
            }

            var urlBook = $"https://localhost:7197/api/TheLoai/EditTL/{id}";

            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllTheLoaiManager", "TheLoai", new { area = "Admin" });

        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatusTheLoaiKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/TheLoai/UpdateStatusTheLoai/{id}?_ctsp=1";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Kinh Doanh: {Error}", errorMessage);
                return BadRequest($"Failed to update status to Kinh Doanh. Error: {errorMessage}");
            }
            return RedirectToAction("AllTheLoaiManager");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusTheLoaiKKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/TheLoai/UpdateStatusTheLoai/{id}?_ctsp=0";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Không Kinh Doanh: {Error}", errorMessage);
                return BadRequest($"Failed to update status to Không Kinh Doanh. Error: {errorMessage}");
            }
            return RedirectToAction("AllTheLoaiManager");
        }
        private async Task<bool> IsDuplicateTheLoai(string tenMauSac, string id = null)
        {
            var urlBook = $"https://localhost:7197/api/TheLoai/GetAllTheLoai";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataBook);

            if (id == null)
            {
                return lstBook.Any(x => x.TenTheLoai == tenMauSac);
            }
            else
            {
                return lstBook.Any(x => x.TenTheLoai == tenMauSac && x.MaTheLoai != id);
            }
        }
    }
}
