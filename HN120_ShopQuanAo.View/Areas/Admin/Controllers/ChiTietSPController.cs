using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class ChiTietSPController : Controller
    {
        private HttpClient _httpClient;
        public ChiTietSPController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7197/api/ChatLieu/GetAllChatLieu

        //https://localhost:7197/api/ChatLieu/update-TH/TH1
        [HttpGet]
        public async Task<IActionResult> AllChiTietSpManager(string id)
        {
            var urlSize = $"https://localhost:7197/api/Size/GetAllSize";
            var responSize = await _httpClient.GetAsync(urlSize);
            string apiDatasz = await responSize.Content.ReadAsStringAsync();
            var lstSZ = JsonConvert.DeserializeObject<List<Size>>(apiDatasz);

            var urlms = $"https://localhost:7197/api/MauSac/GetAllMauSac";
            var responms = await _httpClient.GetAsync(urlms);
            string apiDatams = await responms.Content.ReadAsStringAsync();
            var lstms = JsonConvert.DeserializeObject<List<MauSac>>(apiDatams);
            ViewBag.lstms = lstms;


            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
            //var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDataBook);
            var Book = lstBook.Where(x => x.MaSp == id);
            return View(Book);
        }
        [HttpGet]
        public IActionResult CreateChiTietSp(string id)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateChiTietSp(string id,ChiTietSp bk, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoSanPhamCT", imageFile.FileName);
                var stream = new FileStream(path, FileMode.Create);
                imageFile.CopyTo(stream);
                bk.UrlAnhSpct = imageFile.FileName;
            }
            bk.MaSp = id;
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //bk.MaSp = DateTime.Now;
            var urlBook = $"https://localhost:7197/api/CTSanPham/AddCTSP?MaSp={bk.MaSp}&MaSize={bk.MaSize}&MaMau={bk.MaMau}&MaKhuyenMai={bk.MaKhuyenMai}&UrlAnhSpct={bk.UrlAnhSpct}&DonGia={bk.DonGia}&SoLuongTon={bk.SoLuongTon}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllChiTietSpManager", "ChiTietSp", new { area = "Admin",id=bk.MaSp });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ChiTietSpDetail(string id)
        {
            
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.SKU == id);
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
        public async Task<IActionResult> UpdateChiTietSp(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.SKU == id);
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
        public async Task<IActionResult> UpdateChiTietSp(string id, ChiTietSp vc, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoSanPhamCT", imageFile.FileName);
                var stream = new FileStream(path, FileMode.Create);
                imageFile.CopyTo(stream);
                vc.UrlAnhSpct = imageFile.FileName;
            }

            var urlBook = $"https://localhost:7197/api/CTSanPham/EditCTSP/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllChiTietSpManager", "ChiTietSp", new { area = "Admin", id = vc.MaSp });

        }
    }
}
