using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Controllers
{
    public class KhuyenMaiController : Controller
    {
        private HttpClient _httpClient;
        public KhuyenMaiController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai
        //https://localhost:7197/api/KhuyenMai/add-TL?TenKhuyenMai=1&PhanTramGiam=1&TrangThai=1
        //https://localhost:7197/api/KhuyenMai/update-TL
        [HttpGet]
        public async Task<IActionResult> AllKhuyenMaiManager()
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
            //var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDataBook);
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
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7197/api/KhuyenMai/add-TL?TenKhuyenMai={bk.TenKhuyenMai}&PhanTramGiam={bk.PhanTramGiam}&TrangThai={bk.TrangThai}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllKhuyenMaiManager", "KhuyenMai");
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> KhuyenMaiDetail(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<IActionResult> UpdateKhuyenMai(string id, KhuyenMai vc)
        {
            var urlBook = $"https://localhost:7197/api/KhuyenMai/update-TL/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllKhuyenMaiManager", "KhuyenMai");

        }
    }
}
