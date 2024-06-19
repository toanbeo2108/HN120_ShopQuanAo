using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using HN120_ShopQuanAo.Data.ViewModels;

using static System.Net.WebRequestMethods;
using HN120_ShopQuanAo.View.PhanTrang;
using System.Drawing.Printing;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class VoucherController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext dBContext;
        public VoucherController()
        {
            _httpClient = new HttpClient();
            dBContext = new AppDbContext();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllVoucher()
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/GetAllVoucher";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Voucher>>(apiDataBook);
            return View(lstBook);
        }

        // tim kiem ten
        //[HttpGet]
        //public async Task<IActionResult> TimKiemTenVC(string MaVC, int ProductPage = 1)
        //{
        //    string apiURL = $"https://localhost:7197/GetAllVoucher";
        //    var response = await _httpClient.GetAsync(apiURL);
        //    var apiData = await response.Content.ReadAsStringAsync();
        //    var roles = JsonConvert.DeserializeObject<List<VoucherView>>(apiData);
        //    return View("GetAllVoucher", new PhanTrangVouchers
        //    {
        //        listvouchers = roles.Where(x => x.MaVoucher.Contains(MaVC.Trim()))
        //                .Skip((ProductPage - 1) * PageSize).Take(PageSize),
        //        PagingInfo = new PagingInfo
        //        {
        //            ItemsPerPage = PageSize,
        //            CurrentPage = ProductPage,
        //            TotalItems = roles.Count()
        //        }

        //    }
        //        );

        //}
        // create
        public IActionResult CreateVC()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateVC(Voucher bk)
        {
            var urlBook = $"https://localhost:7197/CreateVCher?MaVoucher={bk.MaVoucher}&DieuKienGiam={bk.DieuKienGiam}&GiaGiamToiThieu={bk.GiaGiamToiThieu}&GiaGiamToiDa={bk.GiaGiamToiDa}&NgayBatDau={bk.NgayBatDau}&NgayKetThuc={bk.NgayKetThuc}&KieuGiamGia={bk.KieuGiamGia}&GiaTriGiam={bk.GiaTriGiam}&SoLuong={bk.SoLuong}&TrangThai={bk.TrangThai}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllVoucher", "Voucher", new { area = "Admin" });
            }
            TempData["error message"] = "thêm thất bại";
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateVC(VoucherView voucher)
        //{
        //    try
        //    {
        //        string apiURL = $"https://localhost:7197/CreateVCher";
        //        var response1 = await _httpClient.GetAsync(apiURL);
        //        var apiData = await response1.Content.ReadAsStringAsync();
        //        var roles = JsonConvert.DeserializeObject<List<VoucherView>>(apiData);

        //        if (voucher.dieuKienGiam != null || voucher.MaVoucher != null || voucher.giaTriGiam != null || voucher.KieuGiamGia != null || voucher.TrangThai != null || voucher.NgayBatDau != null || voucher.NgayKetThuc != null)
        //        {
        //            if (voucher.dieuKienGiam < 0)
        //            {
        //                ViewData["DieuKienGiam"] = "Điều kiện giảm không được âm";
        //            }
        //            if (voucher.giaTriGiam <= 0)
        //            {
        //                ViewData["GiaTriGiam"] = "Mời bạn nhập giá trị giảm lớn hơn 0";
        //            }
        //            if (voucher.giaGiamToiThieu <= 0)
        //            {
        //                ViewData["GiaGiamToiThieu"] = "Mời bạn nhập giá giảm tối thiểu lớn hơn 0";
        //            }
        //            if (voucher.giaGiamToiDa <= 0)
        //            {
        //                ViewData["GiaGiamToiDa"] = "Mời bạn nhập giá giảm tối đa lớn hơn 0";
        //            }
        //            if (voucher.SoLuong <= 0)
        //            {
        //                ViewData["SoLuong"] = "Mời bạn nhập số lượng lớn hơn 0";
        //            }
        //            if (voucher.NgayKetThuc < voucher.NgayBatDau)
        //            {
        //                ViewData["Ngay"] = "Ngày kết thúc phải lớn hơn ngày bắt đầu";
        //            }
        //            var timkiem = roles.FirstOrDefault(x => x.MaVoucher == voucher.MaVoucher.Trim());
        //            if (timkiem != null)
        //            {
        //                ViewData["Ma"] = "Mã này đã tồn tại";
        //            }
        //            if (voucher.KieuGiamGia == 1)
        //            {
        //                if (voucher.dieuKienGiam == 0)
        //                {
        //                    if (voucher.giaTriGiam > 100 || voucher.giaTriGiam <= 0)
        //                    {
        //                        ViewData["GiaTriGiam"] = "Giá trị từ 1 đến 100";
        //                        return View();
        //                    }
        //                    if (voucher.giaTriGiam <= 100 && voucher.giaTriGiam > 0)
        //                    {
        //                        if (voucher.dieuKienGiam >= 0 && voucher.giaTriGiam > 0 && voucher.giaGiamToiThieu > 0 && voucher.giaGiamToiDa > 0 && voucher.SoLuong > 0 && voucher.NgayKetThuc >= voucher.NgayBatDau && timkiem == null)
        //                        {
        //                            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7197/CreateVCher", voucher);
        //                            if (response.IsSuccessStatusCode)
        //                            {
        //                                return RedirectToAction("GetAllVoucher");
        //                            }
        //                            return View();
        //                        }
        //                    }
        //                }
        //                if (voucher.dieuKienGiam > 0)
        //                {
        //                    if (voucher.giaTriGiam <= voucher.dieuKienGiam)
        //                    {
        //                        if (voucher.giaTriGiam <= 100 && voucher.giaTriGiam > 0)
        //                        {
        //                            if (voucher.dieuKienGiam >= 0 && voucher.giaTriGiam > 0 && voucher.giaGiamToiThieu > 0 && voucher.giaGiamToiDa > 0 && voucher.SoLuong > 0 && voucher.NgayKetThuc >= voucher.NgayBatDau && timkiem == null)
        //                            {
        //                                var response = await _httpClient.PostAsJsonAsync($"https://localhost:7197/CreateVCher", voucher);
        //                                if (response.IsSuccessStatusCode)
        //                                {
        //                                    return RedirectToAction("GetAllVoucher");
        //                                }
        //                                return View();
        //                            }
        //                        }
        //                        if (voucher.giaTriGiam > 100 || voucher.giaTriGiam <= 0)
        //                        {
        //                            ViewData["GiaTriGiam"] = "Giá trị từ 1 đến 100";
        //                            return View();
        //                        }
        //                    }
        //                    if (voucher.giaTriGiam > voucher.dieuKienGiam)
        //                    {
        //                        ViewData["GiaTriGiam"] = "Giá trị phải nhỏ hơn hoặc bằng điều kiện giảm";
        //                        return View();
        //                    }
        //                }
        //            }
        //            if (voucher.KieuGiamGia == 0)
        //            {
        //                if (voucher.dieuKienGiam == 0)
        //                {
        //                    if (voucher.giaTriGiam <= 0)
        //                    {
        //                        ViewData["GiaTriGiam"] = "Giá trị phải lớn hơn 0";
        //                        return View();
        //                    }
        //                    if (voucher.giaTriGiam > 0)
        //                    {
        //                        if (voucher.dieuKienGiam >= 0 && voucher.giaTriGiam > 0 && voucher.giaGiamToiThieu > 0 && voucher.giaGiamToiDa > 0 && voucher.SoLuong > 0 && voucher.NgayKetThuc >= voucher.NgayBatDau && timkiem == null)
        //                        {
        //                            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7197/CreateVCher", voucher);
        //                            if (response.IsSuccessStatusCode)
        //                            {
        //                                return RedirectToAction("GetAllVoucher");
        //                            }
        //                            return View();
        //                        }
        //                    }
        //                }
        //                if (voucher.dieuKienGiam > 0)
        //                {
        //                    if (voucher.giaTriGiam <= voucher.dieuKienGiam)
        //                    {
        //                        if (voucher.giaTriGiam <= 0)
        //                        {
        //                            ViewData["GiaTriGiam"] = "Giá trị phải lớn hơn 0";
        //                            return View();
        //                        }
        //                        if (voucher.giaTriGiam > 0)
        //                        {
        //                            if (voucher.dieuKienGiam >= 0 && voucher.giaTriGiam > 0 && voucher.giaGiamToiThieu > 0 && voucher.giaGiamToiDa > 0 && voucher.SoLuong > 0 && voucher.NgayKetThuc >= voucher.NgayBatDau && timkiem == null)
        //                            {
        //                                var response = await _httpClient.PostAsJsonAsync($"https://localhost:7197/CreateVCher", voucher);
        //                                if (response.IsSuccessStatusCode)
        //                                {
        //                                    return RedirectToAction("GetAllVoucher");
        //                                }
        //                                return View();
        //                            }
        //                        }
        //                    }
        //                    if (voucher.giaTriGiam > voucher.dieuKienGiam)
        //                    {
        //                        ViewData["GiaTriGiam"] = "Giá trị phải nhỏ hơn hoặc bằng điều kiện giảm";
        //                        return View();
        //                    }
        //                }
        //            }
        //        }
        //        return View();
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // update
        [HttpGet]
        public IActionResult UpdateVC(Guid id)
        {
            try
            {
                var url = $"https://localhost:7197/UpdateVCher";
                var response = _httpClient.GetAsync(url).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var KhuyenMais = JsonConvert.DeserializeObject<VoucherView>(result);
                return View(KhuyenMais);
            }
            catch
            {
                return View();
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateVC(string id, Voucher vc)
        {
            var urlBook = $"https://localhost:7197/UpdateVCher?MaVoucher={vc.MaVoucher}&DieuKienGiam={vc.DieuKienGiam}&GiaGiamToiThieu={vc.GiaGiamToiThieu}&GiaGiamToiDa={vc.GiaGiamToiDa}&NgayBatDau={vc.NgayBatDau}&NgayKetThuc={vc.NgayKetThuc}&KieuGiamGia={vc.KieuGiamGia}&GiaTriGiam={vc.GiaTriGiam}&SoLuong={vc.SoLuong}&TrangThai={vc.TrangThai}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("GetAllVoucher", "Voucher", new { area = "Admin" });

        }

        public async Task<IActionResult> SuDung(string MaVoucher)
        {
            try
            {
                var timkiem = dBContext.Voucher.FirstOrDefault(x => x.MaVoucher == MaVoucher);
                if (timkiem != null)
                {
                    timkiem.TrangThai = 1;
                    dBContext.Voucher.Update(timkiem);
                    dBContext.SaveChanges();
                    return RedirectToAction("GetAllVoucher");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }

        }
        public async Task<IActionResult> KoSuDung(string MaVoucher)
        {
            try
            {
                var timkiem = dBContext.Voucher.FirstOrDefault(x => x.MaVoucher == MaVoucher);
                if (timkiem != null)
                {
                    timkiem.TrangThai = 0;
                    dBContext.Voucher.Update(timkiem);
                    dBContext.SaveChanges();
                    return RedirectToAction("GetAllVoucher");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }

        }

    }
}
