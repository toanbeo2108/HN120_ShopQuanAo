﻿using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Policy;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        private HttpClient _httpClient;
        private readonly ILogger<SanPhamController> _logger;
        IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SanPhamController(ILogger<SanPhamController> logger, IHttpClientFactory httpClientFactory, IWebHostEnvironment hostEnvironment)
        {
            _httpClient = new HttpClient();
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7197/api/Size/GetAllSize
        //https://localhost:7197/api/Size/add-SZ?Tensz=1&MoTa=1&TrangThai=1
        //    https://localhost:7197/api/Size/update-SZ
        [HttpGet]
        public async Task<IActionResult> AllSanPhamManager(string searchString)


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

            var urlCL = $"https://localhost:7197/api/ChatLieu/GetAllChatLieu";
            var responCL = await _httpClient.GetAsync(urlCL);
            string apiDataCL = await responCL.Content.ReadAsStringAsync();
            var lstCL = JsonConvert.DeserializeObject<List<ChatLieu>>(apiDataCL);
            ViewBag.lstCL = lstCL;


            var urlBook = $"https://localhost:7197/api/SanPham/GetAllSanPham";

            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apiDataBook);

            if (!string.IsNullOrEmpty(searchString))
            {
                lstSP = lstSP.Where(x => x.TenSP.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            lstSP = lstSP.OrderByDescending(x => x.NgayNhap).ToList();
            ViewData["CurrentFilter"] = searchString;
            return View(lstSP);
        }


        [HttpGet]
        public async Task<IActionResult> CreateSanPham()
        {
            await LoadDataForViewBag();
            return View(new AddSpViewModel { ChiTietSps = new List<ChiTietSpInputModel>() });
        }

        [HttpPost]
        public async Task<IActionResult> CreateSanPham(AddSpViewModel model, IFormFile imageSP, List<IFormFile> imagechitietsp)
        {
            // Xử lý ảnh đại diện sản phẩm
            if (imageSP != null && imageSP.Length > 0)
            {
                model.UrlAvatar = await UploadImageAsync(imageSP, "photoSP");
            }

            // Xử lý lưu ảnh chi tiết sản phẩm
            for (int i = 0; i < imagechitietsp.Count; i++)
            {
                var imageFile = imagechitietsp[i];
                if (imageFile != null && imageFile.Length > 0)
                {
                    model.ChiTietSps[i].UrlAnhSpct = await UploadImageAsync(imageFile, "photoSanPhamCT");
                }
            }
            if (await IsDuplicateSP(model.TenSp))
            {
                TempData["errorMessage"] = "Tên đã tồn tại.";
                return View();
            }
            // Gọi API để thêm sản phẩm và chi tiết sản phẩm
            var httpClient = _httpClientFactory.CreateClient();
            var url = "https://localhost:7197/api/SanPham/AddSpWithDetails";
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AllSanPhamManager", "SanPham", new { area = "Admin" });
            }

            TempData["errorMessage"] = "Thêm thất bại";

            // Lấy lại dữ liệu cho ViewBag nếu ModelState không hợp lệ
            await LoadDataForViewBag();

            return View(model);
        }

        private async Task<string> UploadImageAsync(IFormFile imageFile, string folderName)
        {
            var fileName = Path.GetFileName(imageFile.FileName);
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, folderName, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return fileName;
        }

        private async Task LoadDataForViewBag()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var urlTL = "https://localhost:7197/api/TheLoai/GetAllTheLoai";
            var responTL = await httpClient.GetAsync(urlTL);
            string apiDataTL = await responTL.Content.ReadAsStringAsync();
            var lstTL = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataTL);
            ViewBag.lstTL = lstTL;

            var urlTH = "https://localhost:7197/api/ThuongHieu/GetAllThuongHieu";
            var responTH = await httpClient.GetAsync(urlTH);
            string apiDataTH = await responTH.Content.ReadAsStringAsync();
            var lstTH = JsonConvert.DeserializeObject<List<ThuongHieu>>(apiDataTH);
            ViewBag.lstTH = lstTH;

            var urlCL = "https://localhost:7197/api/ChatLieu/GetAllChatLieu";
            var responCL = await httpClient.GetAsync(urlCL);
            string apiDataCL = await responCL.Content.ReadAsStringAsync();
            var lstCL = JsonConvert.DeserializeObject<List<ChatLieu>>(apiDataCL);
            ViewBag.lstCL = lstCL;

            var urlSZ = "https://localhost:7197/api/Size/GetAllSize";
            var responSZ = await httpClient.GetAsync(urlSZ);
            string apiDataSZ = await responSZ.Content.ReadAsStringAsync();
            var lstSZ = JsonConvert.DeserializeObject<List<Size>>(apiDataSZ);
            ViewBag.lstSZ = lstSZ;

            var urlMS = "https://localhost:7197/api/MauSac/GetAllMauSac";
            var responMS = await httpClient.GetAsync(urlMS);
            string apiDataMS = await responMS.Content.ReadAsStringAsync();
            var lstMS = JsonConvert.DeserializeObject<List<MauSac>>(apiDataMS);
            ViewBag.lstMS = lstMS;
        }


        [HttpGet]
        public async Task<IActionResult> SanPhamDetail(string id)
        {
            await LoadDataForViewBag();
            var urlsp = $"https://localhost:7197/api/SanPham/GetAllSanPham";
            var responsp = await _httpClient.GetAsync(urlsp);
            string apiDatasp = await responsp.Content.ReadAsStringAsync();
            var lstsp = JsonConvert.DeserializeObject<List<SanPham>>(apiDatasp);
            var sp = lstsp.FirstOrDefault(x => x.MaSp == id);
            ViewBag.sp = sp;

            var urlkm = "https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
            var responkm = await _httpClient.GetAsync(urlkm);
            responkm.EnsureSuccessStatusCode();
            string apiDatakm = await responkm.Content.ReadAsStringAsync();
            var lstkm = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDatakm);
            ViewBag.lstkm = lstkm;

            var urlctsp = $"https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
            var responctsp = await _httpClient.GetAsync(urlctsp);
            string apiDatactsp = await responctsp.Content.ReadAsStringAsync();
            var lstctsp = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDatactsp);
            var ctsp = lstctsp.Where(x => x.MaSp == id).ToList();
            ViewBag.ctsp = ctsp;
            if (sp == null)
            {
                return BadRequest();
            }
            else
            {
                return View(sp);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateSanPham(string id)
        {
            var urlTL = $"https://localhost:7197/api/TheLoai/GetAllTheLoai";
            //var httpClient = new HttpClient();
            var responTL = await _httpClient.GetAsync(urlTL);
            string apiDataTL = await responTL.Content.ReadAsStringAsync();
            var lstTL = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataTL);
            //var lstTL = lstTL1.Where(c => c.TrangThai == 1);
            ViewBag.lstTL = lstTL;

            var urlTH = $"https://localhost:7197/api/ThuongHieu/GetAllThuongHieu";
            //var httpClient = new HttpClient();
            var responTH = await _httpClient.GetAsync(urlTH);
            string apiDataTH = await responTH.Content.ReadAsStringAsync();
            var lstTH = JsonConvert.DeserializeObject<List<ThuongHieu>>(apiDataTH);
            //var lstTH = lstTH1.Where(c => c.TrangThai == 1);
            ViewBag.lstTH = lstTH;

            var urlCL = $"https://localhost:7197/api/ChatLieu/GetAllChatLieu";
            var responCL = await _httpClient.GetAsync(urlCL);
            string apiDataCL = await responCL.Content.ReadAsStringAsync();
            var lstCL = JsonConvert.DeserializeObject<List<ChatLieu>>(apiDataCL);
            //var lstCL = lstCL1.Where(c => c.TrangThai == 1);
            ViewBag.lstCL = lstCL;
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
            var urlTL = $"https://localhost:7197/api/TheLoai/GetAllTheLoai";
            var responTL = await _httpClient.GetAsync(urlTL);
            string apiDataTL = await responTL.Content.ReadAsStringAsync();
            var lstTL = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataTL);
            ViewBag.lstTL = lstTL;

            var urlTH = $"https://localhost:7197/api/ThuongHieu/GetAllThuongHieu";
            var responTH = await _httpClient.GetAsync(urlTH);
            string apiDataTH = await responTH.Content.ReadAsStringAsync();
            var lstTH = JsonConvert.DeserializeObject<List<ThuongHieu>>(apiDataTH);
            ViewBag.lstTH = lstTH;

            var urlCL = $"https://localhost:7197/api/ChatLieu/GetAllChatLieu";
            var responCL = await _httpClient.GetAsync(urlCL);
            string apiDataCL = await responCL.Content.ReadAsStringAsync();
            var lstCL = JsonConvert.DeserializeObject<List<ChatLieu>>(apiDataCL);
            ViewBag.lstCL = lstCL;

            if (await IsDuplicateSP(vc.TenSP, id))
            {
                TempData["errorMessage"] = "Tên đã tồn tại.";
                return View();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoSP", imageFile.FileName);

                // Kiểm tra và tạo thư mục nếu chưa tồn tại
                var directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                vc.UrlAvatar = imageFile.FileName;
            }

            var urlBook = $"https://localhost:7197/api/SanPham/EditSP/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction("AllSanPhamManager", "SanPham", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusSPKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/SanPham/UpdateStatusSanPham/{id}?_sp=1";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Kinh Doanh: {Error}", errorMessage);
                return BadRequest($"Failed to update status to Kinh Doanh. Error: {errorMessage}");
            }
            return RedirectToAction("AllSanPhamManager");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatusSPKKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/SanPham/UpdateStatusSanPham/{id}?_sp=0";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Không Kinh Doanh: {Error}", errorMessage);
                return BadRequest($"Failed to update status to Không Kinh Doanh. Error: {errorMessage}");
            }
            return RedirectToAction("AllSanPhamManager");
        }
        private async Task<bool> IsDuplicateSP(string tenMauSac, string id = null)
        {
            var urlBook = $"https://localhost:7197/api/SanPham/GetAllSanPham";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<SanPham>>(apiDataBook);

            if (id == null)
            {
                return lstBook.Any(x => x.TenSP == tenMauSac);
            }
            else
            {
                return lstBook.Any(x => x.TenSP == tenMauSac && x.MaSp != id);
            }
        }

    }
}
