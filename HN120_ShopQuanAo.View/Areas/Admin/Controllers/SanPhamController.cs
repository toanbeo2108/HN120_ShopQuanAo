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
        private readonly ILogger<SanPhamController> _logger;
        IHttpClientFactory _httpClientFactory;

        public SanPhamController(ILogger<SanPhamController> logger, IHttpClientFactory httpClientFactory)
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
            await LoadDataForViewBag();

            var viewModel = new AddChiTietSpViewModel
            {
                MaSp = id,
                ChiTietSps = new List<ChiTietSpInputModel>()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddChiTietSp(AddChiTietSpViewModel viewModel, List<IFormFile> imageFiles)
        {
            
                var chiTietSps = new List<ChiTietSp>();

                for (int i = 0; i < viewModel.ChiTietSps.Count; i++)
                {
                    var ctsp = viewModel.ChiTietSps[i];
                    var chiTietSp = new ChiTietSp
                    {
                        SKU = viewModel.MaSp + ctsp.MaSize + ctsp.MaMau,
                        MaSp = viewModel.MaSp,
                        MaSize = ctsp.MaSize,
                        MaMau = ctsp.MaMau,
                        DonGia = ctsp.DonGia,
                        //GiaBan = ctsp.GiaBan,
                        SoLuongTon = ctsp.SoLuongTon,
                        MaKhuyenMai = ctsp.MaKhuyenMai
                    };

                    if (imageFiles.Count > i && imageFiles[i] != null && imageFiles[i].Length > 0)
                    {
                        var imageUrl = await UploadImageAsync(imageFiles[i]);
                        chiTietSp.UrlAnhSpct = imageUrl;
                    }

                    chiTietSps.Add(chiTietSp);
                }

                var url = "https://localhost:7197/api/CTSanPham/AddLstCTSP";
                var content = new StringContent(JsonConvert.SerializeObject(chiTietSps), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AllChiTietSpManager", "ChiTietSp", new { area = "Admin", id = viewModel.MaSp });
                }
            

            await LoadDataForViewBag(); // Reload ViewBag data if ModelState is invalid

            return View(viewModel);
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

        private async Task LoadDataForViewBag()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var urlSize = "https://localhost:7197/api/Size/GetAllSize";
                var responSize = await httpClient.GetAsync(urlSize);
                responSize.EnsureSuccessStatusCode();
                string apiDatasz = await responSize.Content.ReadAsStringAsync();
                var lstSZ = JsonConvert.DeserializeObject<List<Size>>(apiDatasz);
                ViewBag.lstSZ = lstSZ;

                var urlms = "https://localhost:7197/api/MauSac/GetAllMauSac";
                var responms = await httpClient.GetAsync(urlms);
                responms.EnsureSuccessStatusCode();
                string apiDatams = await responms.Content.ReadAsStringAsync();
                var lstms = JsonConvert.DeserializeObject<List<MauSac>>(apiDatams);
                ViewBag.lstms = lstms;

                var urlkm = "https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
                var responkm = await httpClient.GetAsync(urlkm);
                responkm.EnsureSuccessStatusCode();
                string apiDatakm = await responkm.Content.ReadAsStringAsync();
                var lstkm = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDatakm);
                ViewBag.lstkm = lstkm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading data for ViewBag");
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình tải dữ liệu.";
            }
        }
            [HttpGet]
            public async Task<IActionResult> NhapHang(string id)
            {
                //var httpClient = _httpClientFactory.CreateClient();

                // Gọi API để lấy danh sách chi tiết sản phẩm theo mã sản phẩm
                var urlCTSP = $"https://localhost:7197/api/CTSanPham/GetByMaSp?maSp={id}";
                var responCTSP = await _httpClient.GetAsync(urlCTSP);
                string apiDataCTSP = await responCTSP.Content.ReadAsStringAsync();

                // Kiểm tra JSON trả về
                var chiTietSps = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDataCTSP);

                // Chuyển đổi ChiTietSp sang ChiTietSpInputModel
                var chiTietSpInputModels = chiTietSps.Select(ctsp => new ChiTietSpInputModel
                {
                    MaSize = ctsp.MaSize,
                    MaMau = ctsp.MaMau,
                    DonGia = ctsp.DonGia,
                    //GiaBan = ctsp.GiaBan,
                    SoLuongTon = ctsp.SoLuongTon,
                    UrlAnhSpct = ctsp.UrlAnhSpct,
                    MaKhuyenMai = ctsp.MaKhuyenMai
                }).ToList();

                // Gọi API để lấy danh sách khuyến mãi
                var urlKM = $"https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
                var responKM = await _httpClient.GetAsync(urlKM);
                string apiDataKM = await responKM.Content.ReadAsStringAsync();
                var lstKM = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDataKM);
                ViewBag.lstKM = lstKM;

                // Tạo ViewModel và truyền dữ liệu vào View
                var viewModel = new AddChiTietSpViewModel
                {
                    MaSp = id,
                    ChiTietSps = chiTietSpInputModels
                };

                return View(viewModel);
            }

            [HttpPost]
            public async Task<IActionResult> NhapHang(AddChiTietSpViewModel model)
            {
                var urlKM = $"https://localhost:7197/api/KhuyenMai/GetAllKhuyenMai";
                var responKM = await _httpClient.GetAsync(urlKM);
                string apiDataKM = await responKM.Content.ReadAsStringAsync();
                var lstKM = JsonConvert.DeserializeObject<List<KhuyenMai>>(apiDataKM);
                ViewBag.lstKM = lstKM;

                var chiTietSpList = model.ChiTietSps.Select(ctsp => new ChiTietSp
                {
                    MaSp = model.MaSp,
                    MaSize = ctsp.MaSize,
                    MaMau = ctsp.MaMau,
                    DonGia = ctsp.DonGia,
                    //GiaBan = ctsp.GiaBan.HasValue ? ctsp.GiaBan : ctsp.DonGia,
                    SoLuongTon = ctsp.SoLuongTon,
                    UrlAnhSpct = ctsp.UrlAnhSpct,
                    MaKhuyenMai = ctsp.MaKhuyenMai,
                    SKU = model.MaSp + ctsp.MaSize + ctsp.MaMau
                }).ToList();

                var url = "https://localhost:7197/api/CTSanPham/UpdateChiTietSp";
                var content = new StringContent(JsonConvert.SerializeObject(chiTietSpList), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AllSanPhamManager", "SanPham", new { area = "Admin" });
                }

                TempData["errorMessage"] = "Cập nhật thất bại";
                return View(model);
            }

        
        [HttpPost]
        public async Task<IActionResult> CreateSanPham(SanPham bk, IFormFile imageFile)
        {
            // Lấy dữ liệu từ các API khác và gán vào ViewBag
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

            // Kiểm tra tên sản phẩm trùng lặp
            if (await IsDuplicateSP(bk.TenSP))
            {
                TempData["errorMessage"] = "Tên đã tồn tại.";
                return View();
            }

            // Xử lý file ảnh
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoSP", imageFile.FileName);

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
                bk.UrlAvatar = imageFile.FileName;
            }

            // Gửi yêu cầu thêm sản phẩm mới tới API
            var urlBook = $"https://localhost:7197/api/SanPham/AddSP?Tensp={bk.TenSP}&MaThuongHieu={bk.MaThuongHieu}&MaTheLoai={bk.MaTheLoai}&MaChatLieu={bk.MaChatLieu}&MoTa={bk.Mota}&UrlAvatar={bk.UrlAvatar}";
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");

            var respon = await _httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllSanPhamManager", "SanPham", new { area = "Admin" });
            }

            TempData["errorMessage"] = "Thêm thất bại";
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
