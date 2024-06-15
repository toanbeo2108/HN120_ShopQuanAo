using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class MauSacController : Controller
    {
        private HttpClient _httpClient;
        public MauSacController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7197/api/MauSac/GetAllMauSac
        //https://localhost:7197/api/MauSac/add-MS?TenMau=1&MoTa=1&TrangThai=1
        //    https://localhost:7197/api/MauSac/update-MS
        [HttpGet]
        public async Task<IActionResult> AllMauSacManager()
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/MauSac/GetAllMauSac";
            //var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstMauSac = JsonConvert.DeserializeObject<List<MauSac>>(apiDataBook);
            ViewBag.lstMauSac = lstMauSac;
            return View(lstMauSac);
        }
        [HttpGet]
        public IActionResult CreateMauSac()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMauSac(MauSac bk)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7197/api/MauSac/AddMS?TenMau={bk.TenMau}&MoTa={bk.MoTa}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllMauSacManager", "MauSac", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> MauSacDetail(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/MauSac/GetAllMauSac";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<MauSac>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaMau == id);
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
        public async Task<IActionResult> UpdateMauSac(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/MauSac/GetAllMauSac";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<MauSac>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaMau == id);
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
        public async Task<IActionResult> UpdateMauSac(string id, MauSac vc)
        {
            var urlBook = $"https://localhost:7197/api/MauSac/UpdateMS/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllMauSacManager", "MauSac", new { area = "Admin" });

        }
    }
}
