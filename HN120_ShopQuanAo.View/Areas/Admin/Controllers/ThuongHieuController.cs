using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class ThuongHieuController : Controller
    {
        private HttpClient _httpClient;
        public ThuongHieuController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7197/api/ThuongHieu/GetAllThuongHieu
        //https://localhost:7197/api/ThuongHieu/add-TL?Tenth=1&MoTa=1&TrangThai=1
        //https://localhost:7197/api/ThuongHieu/update-TL
        [HttpGet]
        public async Task<IActionResult> AllThuongHieuManager()
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/ThuongHieu/GetAllThuongHieu";
            //var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ThuongHieu>>(apiDataBook);
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
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7197/api/ThuongHieu/add-TL?Tenth={bk.TenThuongHieu}&MoTa={bk.MoTa}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllThuongHieuManager", "ThuongHieu", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ThuongHieuDetail(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        [HttpGet]
        public async Task<IActionResult> UpdateThuongHieu(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
            var urlBook = $"https://localhost:7197/api/ThuongHieu/update-TL/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllThuongHieuManager", "ThuongHieu", new { area = "Admin" });

        }
    }
}
