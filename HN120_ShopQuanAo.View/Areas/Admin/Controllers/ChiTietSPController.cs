﻿using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
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
            ViewBag.lstSZ = lstSZ;

            var urlms = $"https://localhost:7197/api/MauSac/GetAllMauSac";
            var responms = await _httpClient.GetAsync(urlms);
            string apiDatams = await responms.Content.ReadAsStringAsync();
            var lstms = JsonConvert.DeserializeObject<List<MauSac>>(apiDatams);
            ViewBag.lstms = lstms;

            var urlkm = "https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
            var responkm = await _httpClient.GetAsync(urlkm);
            responkm.EnsureSuccessStatusCode();
            string apiDatakm = await responkm.Content.ReadAsStringAsync();
            var lstkm = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDatakm);
            ViewBag.lstkm = lstkm;

            var urlBook = $"https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDataBook);
            var Book = lstBook.Where(x => x.MaSp == id);
            return View(Book);
        }

        [HttpGet]
        public async Task<IActionResult> CreateChiTietSpAsync(string id)
        {
            var urlSize = $"https://localhost:7197/api/Size/GetAllSize";
            var responSize = await _httpClient.GetAsync(urlSize);
            string apiDatasz = await responSize.Content.ReadAsStringAsync();
            var lstSZ = JsonConvert.DeserializeObject<List<Size>>(apiDatasz);
            ViewBag.lstSZ = lstSZ;

            var urlms = $"https://localhost:7197/api/MauSac/GetAllMauSac";
            var responms = await _httpClient.GetAsync(urlms);
            string apiDatams = await responms.Content.ReadAsStringAsync();
            var lstms = JsonConvert.DeserializeObject<List<MauSac>>(apiDatams);
            ViewBag.lstms = lstms;

            var urlkm = "https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
            var responkm = await _httpClient.GetAsync(urlkm);
            responkm.EnsureSuccessStatusCode();
            string apiDatakm = await responkm.Content.ReadAsStringAsync();
            var lstkm = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDatakm);
            ViewBag.lstkm = lstkm;
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> CreateChiTietSp(string id, ChiTietSp bk, IFormFile imageFile)

        {
            var urlSize = $"https://localhost:7197/api/Size/GetAllSize";
            var responSize = await _httpClient.GetAsync(urlSize);
            string apiDatasz = await responSize.Content.ReadAsStringAsync();
            var lstSZ = JsonConvert.DeserializeObject<List<Size>>(apiDatasz);
            ViewBag.lstSZ = lstSZ;

            var urlms = $"https://localhost:7197/api/MauSac/GetAllMauSac";
            var responms = await _httpClient.GetAsync(urlms);
            string apiDatams = await responms.Content.ReadAsStringAsync();
            var lstms = JsonConvert.DeserializeObject<List<MauSac>>(apiDatams);
            ViewBag.lstms = lstms;

            var urlkm = "https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
            var responkm = await _httpClient.GetAsync(urlkm);
            responkm.EnsureSuccessStatusCode();
            string apiDatakm = await responkm.Content.ReadAsStringAsync();
            var lstkm = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDatakm);
            ViewBag.lstkm = lstkm;

            bk.MaSp = id;
            bk.SKU = id + bk.MaSize + bk.MaMau;
            if (await IsDuplicateChiTietSP(bk.SKU))
            {
                TempData["errorMessage"] = "chi tiết sản phẩm này đã tồn tại.";
                return View();
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoSanPhamCT", imageFile.FileName);

                // Kiểm tra và tạo thư mục nếu chưa tồn tại
                var directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Sử dụng 'using' để đảm bảo đóng stream
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Cập nhật URL của ảnh vào thuộc tính
                bk.UrlAnhSpct = imageFile.FileName;
            }



            var urlBook = $"https://localhost:7197/api/CTSanPham/AddCTSP?MaSp={bk.MaSp}&MaSize={bk.MaSize}&MaMau={bk.MaMau}&MaKhuyenMai={bk.MaKhuyenMai}&UrlAnhSpct={bk.UrlAnhSpct}&DonGia={bk.DonGia}&SoLuongTon={bk.SoLuongTon}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllChiTietSpManager", "ChiTietSp", new { area = "Admin", id = bk.MaSp });
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
            var chitietsp = lstBook.FirstOrDefault(x => x.SKU == id);
            ViewBag.chitietsp = chitietsp;
            if (chitietsp == null)
            {
                return BadRequest();
            }
            else
            {
                return View(chitietsp);
            }
        }
        //[HttpGet]
        //public async Task<IActionResult> UpdateChiTietSp(string id)
        //{
        //    var urlSize = $"https://localhost:7197/api/Size/GetAllSize";
        //    var responSize = await _httpClient.GetAsync(urlSize);
        //    string apiDatasz = await responSize.Content.ReadAsStringAsync();
        //    var lstSZ = JsonConvert.DeserializeObject<List<Size>>(apiDatasz);
        //    ViewBag.lstSZ = lstSZ;

        //    var urlms = $"https://localhost:7197/api/MauSac/GetAllMauSac";
        //    var responms = await _httpClient.GetAsync(urlms);
        //    string apiDatams = await responms.Content.ReadAsStringAsync();
        //    var lstms = JsonConvert.DeserializeObject<List<MauSac>>(apiDatams);
        //    ViewBag.lstms = lstms;

        //    var urlkm = "https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
        //    var responkm = await _httpClient.GetAsync(urlkm);
        //    responkm.EnsureSuccessStatusCode();
        //    string apiDatakm = await responkm.Content.ReadAsStringAsync();
        //    var lstkm = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDatakm);
        //    ViewBag.lstkm = lstkm;
        //    //var token = Request.Cookies["Token"];
        //    //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var urlBook = $"https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
        //    var responBook = await _httpClient.GetAsync(urlBook);
        //    string apiDataBook = await responBook.Content.ReadAsStringAsync();
        //    var lstBook = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDataBook);
        //    var chitietsp = lstBook.FirstOrDefault(x => x.SKU == id);
        //    ViewBag.chitietsp = chitietsp;
        //    if (chitietsp == null)
        //    {
        //        return BadRequest();
        //    }
        //    else
        //    {
        //        return View(chitietsp);
        //    }
        //}
        [HttpPost,Route("/UpdateChiTietSp")]
        public async Task<IActionResult> UpdateChiTietSp(ChiTietSp ctsp, IFormFile imagectspFile)
        {

            var urlSize = $"https://localhost:7197/api/Size/GetAllSize";
            var responSize = await _httpClient.GetAsync(urlSize);
            string apiDatasz = await responSize.Content.ReadAsStringAsync();
            var lstSZ = JsonConvert.DeserializeObject<List<Size>>(apiDatasz);
            ViewBag.lstSZ = lstSZ;

            var urlms = $"https://localhost:7197/api/MauSac/GetAllMauSac";
            var responms = await _httpClient.GetAsync(urlms);
            string apiDatams = await responms.Content.ReadAsStringAsync();
            var lstms = JsonConvert.DeserializeObject<List<MauSac>>(apiDatams);
            ViewBag.lstms = lstms;

            var urlkm = "https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
            var responkm = await _httpClient.GetAsync(urlkm);
            responkm.EnsureSuccessStatusCode();
            string apiDatakm = await responkm.Content.ReadAsStringAsync();
            var lstkm = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDatakm);
            ViewBag.lstkm = lstkm;

            var spUrl = $"https://localhost:7197/api/CTSanPham/GetCTSPById?id={ctsp.SKU}";
            var spResponse = await _httpClient.GetAsync(spUrl);
            if (!spResponse.IsSuccessStatusCode)
            {
                return BadRequest("Không thể lấy thông tin sản phẩm");
            }
            string spData = await spResponse.Content.ReadAsStringAsync();
            var ExsitSP = JsonConvert.DeserializeObject<ChiTietSp>(spData);


            if (imagectspFile != null && imagectspFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoSanPhamCT", imagectspFile.FileName);

                // Kiểm tra và tạo thư mục nếu chưa tồn tại
                var directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Sử dụng 'using' để đảm bảo đóng stream
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imagectspFile.CopyToAsync(stream);
                }

                // Cập nhật URL của ảnh vào thuộc tính
                ExsitSP.UrlAnhSpct = imagectspFile.FileName;
                ExsitSP.DonGia = ctsp.DonGia;
                ExsitSP.GiaBan = ctsp.GiaBan;
                ExsitSP.SoLuongTon = ctsp.SoLuongTon;
            }
            else
            {
                ExsitSP.DonGia = ctsp.DonGia;
                ExsitSP.GiaBan = ctsp.GiaBan;
                ExsitSP.SoLuongTon = ctsp.SoLuongTon;
            }


            var urlBook = $"https://localhost:7197/api/CTSanPham/EditCTSP/{ctsp.SKU}";

            var content = new StringContent(JsonConvert.SerializeObject(ExsitSP), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("UpdateSanPham", "SanPham", new { area = "Admin", id = ctsp.MaSp });

        }
        private async Task<bool> IsDuplicateChiTietSP(string SKU)
        {
            var urlBook = $"https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDataBook);


            return lstBook.Any(x => x.SKU == SKU);

        }
    }
}
