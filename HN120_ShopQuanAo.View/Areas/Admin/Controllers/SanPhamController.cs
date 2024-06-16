using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        private HttpClient _httpClient;
        public SanPhamController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7197/api/Size/GetAllSize
        //https://localhost:7197/api/Size/add-SZ?Tensz=1&MoTa=1&TrangThai=1
        //    https://localhost:7197/api/Size/update-SZ
        [HttpGet]
        public async Task<IActionResult> AllSanPhamManager()
        {
            var urlTL = $"https://localhost:7197/api/TheLoai/GetAllTheLoai";
            //var httpClient = new HttpClient();
            var responTL = await _httpClient.GetAsync(urlTL);
            string apiDataTL = await responTL.Content.ReadAsStringAsync();
            var lstTL = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataTL);
            ViewBag.lstTL = lstTL;

            var urlTH = $"https://localhost:7197/api/ThuongHieu/GetAllThuongHieu";
            //var httpClient = new HttpClient();
            var responTH = await _httpClient.GetAsync(urlTH);
            string apiDataTH = await responTH.Content.ReadAsStringAsync();
            var lstTH = JsonConvert.DeserializeObject<List<ThuongHieu>>(apiDataTH);
            ViewBag.lstTH = lstTH;

            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/SanPham/GetAllSanPham";
            //var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apiDataBook);
            ViewBag.lstSP = lstSP;
            return View(lstSP);
        }
        [HttpGet]
        public async Task<IActionResult> CreateSanPham()
        {
            var urlTL = $"https://localhost:7197/api/TheLoai/GetAllTheLoai";
            //var httpClient = new HttpClient();
            var responTL = await _httpClient.GetAsync(urlTL);
            string apiDataTL = await responTL.Content.ReadAsStringAsync();
            var lstTL = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataTL);
            ViewBag.lstTL = lstTL;

            var urlTH = $"https://localhost:7197/api/ThuongHieu/GetAllThuongHieu";
            //var httpClient = new HttpClient();
            var responTH = await _httpClient.GetAsync(urlTH);
            string apiDataTH = await responTH.Content.ReadAsStringAsync();
            var lstTH = JsonConvert.DeserializeObject<List<ThuongHieu>>(apiDataTH);
            ViewBag.lstTH = lstTH;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSanPham(SanPham bk, IFormFile imageFile)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //bk.CreateDate = DateTime.Now;
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoBooks", imageFile.FileName);
                var stream = new FileStream(path, FileMode.Create);
                imageFile.CopyTo(stream);
                bk.UrlAvatar = imageFile.FileName;
            }
            var urlBook = $"https://localhost:7197/api/SanPham/AddSP?Tensp={bk.TenSP}&MaThuongHieu={bk.MaThuongHieu}&MaTheLoai={bk.MaTheLoai}&MoTa={bk.Mota}&UrlAvatar={bk.UrlAvatar}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllSanPhamManager", "SanPham", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SanPhamDetail(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/SanPham/GetAllSanPham";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<SanPham>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaSp == id);
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
        public async Task<IActionResult> UpdateSanPham(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/SanPham/GetAllSanPham";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<SanPham>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaSp == id);
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
        public async Task<IActionResult> UpdateSanPham(string id, SanPham vc, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoSanPham", imageFile.FileName);
                var stream = new FileStream(path, FileMode.Create);
                imageFile.CopyTo(stream);
                vc.UrlAvatar = imageFile.FileName;
            }
            var urlBook = $"https://localhost:7197/api/SanPham/UpdateSP/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllSanPhamManager", "SanPham", new { area = "Admin" });

        }
    }
}
