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
        private  HttpClient _httpClient;
        private  AppDbContext _context;
        public VoucherController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllVoucher()
        {
            var urlBook = $"https://localhost:7197/GetAllVoucher";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Voucher>>(apiDataBook);
            return View(lstBook);
        }

        // create
        public IActionResult CreateVC()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateVC(Voucher bk)
        //{
        //    var urlBook = $"https://localhost:7197/CreateVCher?MaVoucher={bk.MaVoucher}&Ten={bk.Ten}&GiaGiamToiThieu={bk.GiaGiamToiThieu}&GiaGiamToiDa={bk.GiaGiamToiDa}&NgayBatDau={bk.NgayBatDau}&NgayKetThuc={bk.NgayKetThuc}&KieuGiamGia={bk.KieuGiamGia}&GiaTriGiam={bk.GiaTriGiam}&SoLuong={bk.SoLuong}&MoTa={bk.MoTa}&TrangThai={bk.TrangThai}";
        //    var httpClient = new HttpClient();
        //    var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
        //    var respon = await httpClient.PostAsync(urlBook, content);
        //    if (respon.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("GetAllVoucher", "Voucher", new { areas = "Admin" });
        //    }
        //    TempData["error message"] = "thêm thất bại";
        //    return View();
        //}
        [HttpPost]
        public async Task<IActionResult> CreateVC(Voucher bk)
        {
            try
            {
                // Kiểm tra các điều kiện nhập dữ liệu từ người dùng
                if (string.IsNullOrEmpty(bk.MaVoucher) || string.IsNullOrEmpty(bk.Ten) ||
                    bk.KieuGiamGia == null || bk.GiaGiamToiThieu == null ||
                    bk.GiaGiamToiDa == null || bk.NgayBatDau == null ||
                    bk.NgayKetThuc == null || bk.GiaTriGiam == null ||
                    bk.SoLuong == null || bk.TrangThai == null)
                {
                    TempData["error message"] = "Vui lòng nhập đầy đủ thông tin.";
                    return View(bk);
                }
                if (bk.GiaGiamToiThieu < 0)
                {
                    ModelState.AddModelError("GiaGiamToiThieu", "Giá giảm tối thiểu không được âm");
                }
                if (bk.GiaGiamToiDa < 0)
                {
                    ModelState.AddModelError("GiaGiamToiDa", "Giá giảm tối đa không được âm");
                }
                if (bk.GiaGiamToiThieu > bk.GiaGiamToiDa)
                {
                    ModelState.AddModelError("GiaGiamToiThieu", "Giá giảm tối thiểu không được lớn hơn giá giảm tối đa");
                }
                if (bk.GiaTriGiam <= 0)
                {
                    ModelState.AddModelError("GiaTriGiam", "Mời bạn nhập giá trị giảm lớn hơn 0");
                }
                if (bk.SoLuong <= 0)
                {
                    ModelState.AddModelError("SoLuong", "Mời bạn nhập số lượng lớn hơn 0");
                }
                if (bk.NgayKetThuc < bk.NgayBatDau)
                {
                    ModelState.AddModelError("NgayKetThuc", "Ngày kết thúc phải lớn hơn ngày bắt đầu");
                }

                if (bk.KieuGiamGia == 1) 
                {
                    if (bk.GiaTriGiam > 100 || bk.GiaTriGiam <= 0)
                    {
                        ModelState.AddModelError("GiaTriGiam", "Giá trị giảm phải từ 1 đến 100");
                    }
                }
                else if (bk.KieuGiamGia == 0) 
                {
                    if (bk.GiaTriGiam <= 0)
                    {
                        ModelState.AddModelError("GiaTriGiam", "Giá trị giảm phải lớn hơn 0");
                    }
                    if (bk.GiaTriGiam < bk.GiaGiamToiThieu || bk.GiaTriGiam > bk.GiaGiamToiDa)
                    {
                        ModelState.AddModelError("GiaTriGiam", "Giá trị giảm phải nằm trong khoảng giá giảm tối thiểu và tối đa");
                    }
                }

                if (!ModelState.IsValid)
                {
                    return View(bk);
                }

                string apiURL = "https://localhost:7197/GetAllVoucher";
                var response1 = await _httpClient.GetAsync(apiURL);
                var apiData = await response1.Content.ReadAsStringAsync();
                var vouchers = JsonConvert.DeserializeObject<List<Voucher>>(apiData);
                var timkiem = vouchers.FirstOrDefault(x => x.MaVoucher == bk.MaVoucher.Trim());
                if (timkiem != null)
                {
                    ModelState.AddModelError("MaVoucher", "Mã này đã tồn tại");
                    return View(bk);
                }

                var urlBook = $"https://localhost:7197/CreateVCher?MaVoucher={bk.MaVoucher}&Ten={bk.Ten}&GiaGiamToiThieu={bk.GiaGiamToiThieu}&GiaGiamToiDa={bk.GiaGiamToiDa}&NgayBatDau={bk.NgayBatDau}&NgayKetThuc={bk.NgayKetThuc}&KieuGiamGia={bk.KieuGiamGia}&GiaTriGiam={bk.GiaTriGiam}&SoLuong={bk.SoLuong}&MoTa={bk.MoTa}&TrangThai={bk.TrangThai}";
                var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(urlBook, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllVoucher", "Voucher", new { areas = "Admin" });
                }

                TempData["error message"] = "Thêm thất bại";
                return View(bk);
            }
            catch (Exception ex)
            {
                TempData["error message"] = "Có lỗi xảy ra: " + ex.Message;
                return View(bk);
            }
        }

        // update
        [HttpGet]
        public async Task<IActionResult> UpdateVC(string id)
        {
            var urlBook = $"https://localhost:7197/GetAllVoucher";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Voucher>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaVoucher == id);
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
        public async Task<IActionResult> UpdateVC(string id, Voucher vc)
        {
            var urlBook = $"https://localhost:7197/UpdateVCher?MaVoucher={vc.MaVoucher}&Ten={vc.Ten}&GiaGiamToiThieu={vc.GiaGiamToiThieu}&GiaGiamToiDa={vc.GiaGiamToiDa}&NgayBatDau={vc.NgayBatDau}&NgayKetThuc={vc.NgayKetThuc}&KieuGiamGia={vc.KieuGiamGia}&GiaTriGiam={vc.GiaTriGiam}&SoLuong={vc.SoLuong}&MoTa={vc.MoTa}&TrangThai={vc.TrangThai}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            return RedirectToAction("GetAllVoucher", "Voucher", new { area = "Admin" });

        }

        public async Task<IActionResult> SuDung(string MaVoucher)
        {
            try
            {
                var timkiem = _context.Voucher.FirstOrDefault(x => x.MaVoucher == MaVoucher);
                if (timkiem != null)
                {
                    timkiem.TrangThai = 0;
                    _context.Voucher.Update(timkiem);
                    await _context.SaveChangesAsync(); 
                    return RedirectToAction("GetAllVoucher");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Logging lỗi nếu cần
                return View();
            }
        }

        public async Task<IActionResult> KoSuDung(string MaVoucher)
        {
            try
            {
                var timkiem = _context.Voucher.FirstOrDefault(x => x.MaVoucher == MaVoucher);
                if (timkiem != null)
                {
                    timkiem.TrangThai = 1;
                    _context.Voucher.Update(timkiem);
                    await _context.SaveChangesAsync(); // Sử dụng SaveChangesAsync thay cho SaveChanges
                    return RedirectToAction("GetAllVoucher"); // Điều hướng về action GetAllVoucher sau khi cập nhật thành công
                }
                else
                {
                    return View(); // Trả về view mặc định nếu không tìm thấy voucher
                }
            }
            catch (Exception ex)
            {
                // Logging lỗi nếu cần
                return View(); // Xử lý lỗi và trả về view mặc định
            }
        }


    }

}

