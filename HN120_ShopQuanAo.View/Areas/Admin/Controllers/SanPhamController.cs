using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Policy;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        private HttpClient _httpClient;
        private readonly ILogger<SizeController> _logger;
        IHttpClientFactory _httpClientFactory;

        public SanPhamController(ILogger<SizeController> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClient = new HttpClient();
            _logger = logger;
            _httpClientFactory = httpClientFactory;
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

            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/SanPham/GetAllSanPham";
            //var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apiDataBook);
            //ViewBag.lstSP = lstSP;
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

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddChiTietSp(string id)
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

            var urlkm = $"https://localhost:7197/api/khuyenmai/getallkhuyenmai";
            var responkm = await _httpClient.GetAsync(urlkm);
            string apiDatakm = await responkm.Content.ReadAsStringAsync();
            var lstkm = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDatakm);
            ViewBag.lstkm = lstkm;

            var model = new AddChiTietSpViewModel { MaSp = id };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddChiTietSp(AddChiTietSpViewModel model)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var urlSize = "https://localhost:7197/api/Size/GetAllSize";
            var responSize = await httpClient.GetAsync(urlSize);
            string apiDatasz = await responSize.Content.ReadAsStringAsync();
            var lstSZ = JsonConvert.DeserializeObject<List<Size>>(apiDatasz);
            ViewBag.lstSZ = lstSZ;

            var urlms = "https://localhost:7197/api/MauSac/GetAllMauSac";
            var responms = await httpClient.GetAsync(urlms);
            string apiDatams = await responms.Content.ReadAsStringAsync();
            var lstms = JsonConvert.DeserializeObject<List<MauSac>>(apiDatams);
            ViewBag.lstms = lstms;

            if (model.SelectedSizes != null && model.SelectedMaus != null)
            {
                model.TempChiTietSps = new List<TempChiTietSpViewModel>();

                foreach (var size in model.SelectedSizes)
                {
                    var sizeDetail = lstSZ.FirstOrDefault(s => s.MaSize == size);
                    foreach (var mau in model.SelectedMaus)
                    {
                        var mauDetail = lstms.FirstOrDefault(m => m.MaMau == mau);
                        model.TempChiTietSps.Add(new TempChiTietSpViewModel
                        {
                            MaSp = model.MaSp,
                            MaSize = size,
                            TenSize = sizeDetail?.TenSize,
                            MaMau = mau,
                            TenMau = mauDetail?.TenMau
                        });
                    }
                }
            }

            await LoadDataForViewBag();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveChiTietSps(List<TempChiTietSpViewModel> models, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                var chiTietSpList = new List<ChiTietSp>();

                for (int i = 0; i < models.Count; i++)
                {
                    var model = models[i];
                    var chiTietSp = new ChiTietSp
                    {
                        MaSp = model.MaSp,
                        MaSize = model.MaSize,
                        MaMau = model.MaMau,
                        DonGia = model.DonGia,
                        GiaBan = model.GiaBan,
                        SoLuongTon = model.SoLuongTon,
                        MaKhuyenMai = model.MaKhuyenMai
                    };

                    if (imageFiles.Count > i && imageFiles[i] != null && imageFiles[i].Length > 0)
                    {
                        var imageUrl = await UploadImageAsync(imageFiles[i]);
                        chiTietSp.UrlAnhSpct = imageUrl;
                    }

                    chiTietSpList.Add(chiTietSp);
                }

                var success = await AddChiTietSpToApi(chiTietSpList);
                if (success)
                {
                    TempData["SuccessMessage"] = "Thêm chi tiết sản phẩm thành công.";
                    return RedirectToAction("AllChiTietSpManager", "ChiTietSp", new { area = "Admin", id = models.FirstOrDefault()?.MaSp });
                }

                TempData["ErrorMessage"] = "Thêm chi tiết sản phẩm thất bại.";
            }

            return View(models);
        }

        private async Task LoadDataForViewBag()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var urlSize = "https://localhost:7197/api/Size/GetAllSize";
            var responSize = await httpClient.GetAsync(urlSize);
            string apiDatasz = await responSize.Content.ReadAsStringAsync();
            var lstSZ = JsonConvert.DeserializeObject<List<Size>>(apiDatasz);
            ViewBag.lstSZ = lstSZ;

            var urlms = "https://localhost:7197/api/MauSac/GetAllMauSac";
            var responms = await httpClient.GetAsync(urlms);
            string apiDatams = await responms.Content.ReadAsStringAsync();
            var lstms = JsonConvert.DeserializeObject<List<MauSac>>(apiDatams);
            ViewBag.lstms = lstms;

            var urlkm = "https://localhost:7197/api/khuyenmai/getallkhuyenmai";
            var responkm = await httpClient.GetAsync(urlkm);
            string apiDatakm = await responkm.Content.ReadAsStringAsync();
            var lstkm = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDatakm);
            ViewBag.lstkm = lstkm;
        }

        private async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            var fileName = Path.GetFileName(imageFile.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoSanPhamCT", fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return fileName;
        }

        private async Task<bool> AddChiTietSpToApi(List<ChiTietSp> chiTietSpList)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = "https://localhost:7197/api/CTSanPham/AddLstCTSP";
            var content = new StringContent(JsonConvert.SerializeObject(chiTietSpList), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSanPham(SanPham bk, IFormFile imageFile)
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
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (await IsDuplicateSP(bk.TenSP))
            {
                TempData["errorMessage"] = "Tên đã tồn tại.";
                return View();
            }
            //bk.CreateDate = DateTime.Now;
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoSP", imageFile.FileName);
                var stream = new FileStream(path, FileMode.Create);
                imageFile.CopyTo(stream);
                bk.UrlAvatar = imageFile.FileName;
            }
            var urlBook = $"https://localhost:7197/api/SanPham/AddSP?Tensp={bk.TenSP}&MaThuongHieu={bk.MaThuongHieu}&MaTheLoai={bk.MaTheLoai}&MaChatLieu={bk.MaChatLieu}&MoTa={bk.Mota}&UrlAvatar={bk.UrlAvatar}";
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
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/SanPham/GetAllSanPham";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<SanPham>>(apiDataBook);
            var sp = lstBook.FirstOrDefault(x => x.MaSp == id);
            ViewBag.sp = sp;
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

            if (await IsDuplicateSP(vc.TenSP, id))
            {
                TempData["errorMessage"] = "Tên đã tồn tại.";
                return View();
            }
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoSP", imageFile.FileName);
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
