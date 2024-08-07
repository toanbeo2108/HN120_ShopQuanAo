﻿using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using HN120_ShopQuanAo.Data.ViewModels;
using static System.Net.WebRequestMethods;
using System.Drawing.Printing;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class VoucherController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VoucherController> _logger;
        private readonly AppDbContext _context;
        public VoucherController(ILogger<VoucherController> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
            _context = new AppDbContext();
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET: Hiển thị tất cả voucher
        [HttpGet]
        public async Task<IActionResult> AllVoucherManager()
        {
            try
            {
                var urlBook = $"https://localhost:7197/GetAllVoucher";
                var responseBook = await _httpClient.GetAsync(urlBook);

                if (responseBook.IsSuccessStatusCode)
                {
                    string apiDataBook = await responseBook.Content.ReadAsStringAsync();
                    var lstBook = JsonConvert.DeserializeObject<List<Voucher>>(apiDataBook);

                    // Cập nhật trạng thái voucher
                    DateTime now = DateTime.Now;
                    foreach (var voucher in lstBook)
                    {
                        if (now < voucher.NgayBatDau)
                        {
                            voucher.TrangThai = 0; // Voucher sắp diễn ra
                        }
                        else if (now >= voucher.NgayBatDau && now < voucher.NgayKetThuc)
                        {
                            if (voucher.TrangThai == 3)
                            {
                                continue;
                            }
                            voucher.TrangThai = 1; // Voucher đang hoạt động
                        }
                        else if (now >= voucher.NgayKetThuc)
                        {
                            voucher.TrangThai = 2; // Voucher đã kết thúc
                        }
                    }

                    lstBook = lstBook.OrderByDescending(v => v.MaVoucher).ToList();
                    return View(lstBook);
                }
                else
                {
                    TempData["error message"] = "Lỗi khi lấy danh sách voucher từ API.";
                    return View(new List<Voucher>());
                }
            }
            catch (Exception ex)
            {
                TempData["error message"] = "Có lỗi xảy ra: " + ex.Message;
                return View(new List<Voucher>());
            }
        }
        

        public IActionResult CreateVC()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVC(Voucher bk)
        {
            try
            {
                if (string.IsNullOrEmpty(bk.MaVoucher) || string.IsNullOrEmpty(bk.Ten) ||
                    bk.KieuGiamGia == null || bk.GiaGiamToiThieu == null ||
                    bk.GiaGiamToiDa == null || bk.NgayBatDau == null ||
                    bk.NgayKetThuc == null || bk.GiaTriGiam == null ||
                    bk.SoLuong == null || bk.TrangThai == null)
                {
                    TempData["error message"] = "Vui lòng nhập đầy đủ thông tin.";
                    return View(bk);
                }
                if (await IsVoucherNameDuplicate(bk.Ten))
                {
                    TempData["error message"] = "Tên voucher đã tồn tại.";
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

                if (bk.GiaGiamToiDa > bk.GiaGiamToiThieu)
                {
                    ModelState.AddModelError("GiaGiamToiThieu", "Giá giảm tối đa không được lớn hơn giá giảm tối thiểu");
                }

                if (bk.GiaTriGiam <= 0)
                {
                    ModelState.AddModelError("GiaTriGiam", "Mời bạn nhập giá trị giảm lớn hơn 0");
                }

                if (bk.SoLuong <= 0 || bk.SoLuong > 1000)
                {
                    ModelState.AddModelError("SoLuong", "Số lượng phải lớn hơn 0 và không được vượt quá 1000");
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
                }

                if (!ModelState.IsValid)
                {
                    return View(bk);
                }

                DateTime now = DateTime.Now;
                if (now < bk.NgayBatDau)
                {
                    bk.TrangThai = 0; // Voucher sắp diễn ra
                }
                else if (now >= bk.NgayBatDau && now < bk.NgayKetThuc)
                {
                    bk.TrangThai = 1; // Voucher đang hoạt động
                }
                else if (now >= bk.NgayKetThuc)
                {
                    bk.TrangThai = 2; // Voucher đã kết thúc
                }

                string createUrl = $"https://localhost:7197/CreateVCher?MaVoucher={bk.MaVoucher}&Ten={bk.Ten}&GiaGiamToiThieu={bk.GiaGiamToiThieu}&GiaGiamToiDa={bk.GiaGiamToiDa}&NgayBatDau={bk.NgayBatDau}&NgayKetThuc={bk.NgayKetThuc}&KieuGiamGia={bk.KieuGiamGia}&GiaTriGiam={bk.GiaTriGiam}&SoLuong={bk.SoLuong}&MoTa={bk.MoTa}&TrangThai={bk.TrangThai}";
                var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(createUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    var getAllUrl = "https://localhost:7197/GetAllVoucher";
                    var responseGetAll = await _httpClient.GetAsync(getAllUrl);

                    if (responseGetAll.IsSuccessStatusCode)
                    {
                        var apiData = await responseGetAll.Content.ReadAsStringAsync();
                        var lstBook = JsonConvert.DeserializeObject<List<Voucher>>(apiData);
                        lstBook = lstBook.OrderByDescending(v => v.MaVoucher).ToList();
                        return RedirectToAction("AllVoucherManager", lstBook);
                    }
                    else
                    {
                        TempData["error message"] = "Lỗi khi lấy danh sách voucher từ API sau khi thêm mới.";
                        return RedirectToAction("AllVoucherManager");
                    }
                }
                else
                {
                    TempData["error message"] = "Thêm mới voucher thất bại.";
                    return View(bk);
                }
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
        public async Task<IActionResult> UpdateVC(Voucher vc)
        {
            try
            {
                if (string.IsNullOrEmpty(vc.Ten) || vc.KieuGiamGia == null ||
                    vc.GiaGiamToiThieu == null || vc.GiaGiamToiDa == null ||
                    vc.NgayBatDau == null || vc.NgayKetThuc == null ||
                    vc.GiaTriGiam == null || vc.SoLuong == null || vc.TrangThai == null)
                {
                    TempData["error message"] = "Vui lòng nhập đầy đủ thông tin.";
                    return View(vc);
                }
                if (await IsVoucherNameDuplicate(vc.Ten, vc.MaVoucher))
                {
                    TempData["error message"] = "Tên voucher đã tồn tại.";
                    return View(vc);
                }

                if (vc.GiaGiamToiThieu < 0)
                {
                    ModelState.AddModelError("GiaGiamToiThieu", "Giá giảm tối thiểu không được âm");
                }

                if (vc.GiaGiamToiDa < 0)
                {
                    ModelState.AddModelError("GiaGiamToiDa", "Giá giảm tối đa không được âm");
                }

                if (vc.GiaGiamToiDa > vc.GiaGiamToiThieu)
                {
                    ModelState.AddModelError("GiaGiamToiThieu", "Giá giảm tối đa không được lớn hơn giá giảm tối thiểu");
                }

                if (vc.GiaTriGiam <= 0)
                {
                    ModelState.AddModelError("GiaTriGiam", "Mời bạn nhập giá trị giảm lớn hơn 0");
                }

                if (vc.SoLuong <= 0)
                {
                    ModelState.AddModelError("SoLuong", "Mời bạn nhập số lượng lớn hơn 0");
                }

                if (vc.NgayKetThuc < vc.NgayBatDau)
                {
                    ModelState.AddModelError("NgayKetThuc", "Ngày kết thúc phải lớn hơn ngày bắt đầu");
                }

                if (vc.KieuGiamGia == 1)
                {
                    if (vc.GiaTriGiam > 100 || vc.GiaTriGiam <= 0)
                    {
                        ModelState.AddModelError("GiaTriGiam", "Giá trị giảm phải từ 1 đến 100");
                    }
                }
                else if (vc.KieuGiamGia == 0)
                {
                    if (vc.GiaTriGiam <= 0)
                    {
                        ModelState.AddModelError("GiaTriGiam", "Giá trị giảm phải lớn hơn 0");
                    }
                    //if (vc.GiaTriGiam < vc.GiaGiamToiThieu || vc.GiaTriGiam > vc.GiaGiamToiDa)
                    //{
                    //    ModelState.AddModelError("GiaTriGiam", "Giá trị giảm phải nằm trong khoảng giá giảm tối thiểu và tối đa");
                    //}
                }

                if (!ModelState.IsValid)
                {
                    return View(vc);
                }

                // Tính toán và đặt trạng thái của voucher
                DateTime now = DateTime.Now;
                if (now < vc.NgayBatDau)
                {
                    vc.TrangThai = 0; // Voucher sắp diễn ra
                }
                else if (now >= vc.NgayBatDau && now < vc.NgayKetThuc)
                {
                    vc.TrangThai = 1; // Voucher đang hoạt động
                }
                else if (now >= vc.NgayKetThuc)
                {
                    vc.TrangThai = 2; // Voucher đã kết thúc
                }

                var urlBook = $"https://localhost:7197/UpdateVCher/{vc.MaVoucher}";
                var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
                var respon = await _httpClient.PutAsync(urlBook, content);
                if (!respon.IsSuccessStatusCode)
                {
                    TempData["error message"] = "Cập nhật thất bại";
                    return View(vc);
                }
                else
                {
                    return RedirectToAction("AllVoucherManager", "Voucher", new { Areas = "Admin" });
                }

            }
            catch (Exception ex)
            {
                TempData["error message"] = "Có lỗi xảy ra: " + ex.Message;
                return View(vc);
            }

        }
        [HttpPost]
        public async Task<IActionResult> EndEarly(string id)
        {
            try
            {
                var urlBook = $"https://localhost:7197/GetAllVoucher";
                var responseBook = await _httpClient.GetAsync(urlBook);
                string apiDataBook = await responseBook.Content.ReadAsStringAsync();
                var lstBook = JsonConvert.DeserializeObject<List<Voucher>>(apiDataBook);
                var voucher = lstBook.FirstOrDefault(x => x.MaVoucher == id);

                if (voucher == null)
                {
                    TempData["error message"] = "Voucher không tồn tại.";
                    return RedirectToAction("AllVoucherManager");
                }

                voucher.TrangThai = 3;

                var urlUpdate = $"https://localhost:7197/UpdateVCher/{voucher.MaVoucher}";
                var content = new StringContent(JsonConvert.SerializeObject(voucher), Encoding.UTF8, "application/json");
                var responseUpdate = await _httpClient.PutAsync(urlUpdate, content);

                if (!responseUpdate.IsSuccessStatusCode)
                {
                    TempData["error message"] = "Cập nhật thất bại.";
                }
                else
                {
                    TempData["success message"] = "Voucher đã được cập nhật thành 'Kết thúc sớm'.";
                }

                return RedirectToAction("AllVoucherManager");
            }
            catch (Exception ex)
            {
                TempData["error message"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("AllVoucherManager");
            }
        }
        private async Task<bool> IsVoucherNameDuplicate(string voucherName, string currentVoucherId = null)
        {
            var url = $"https://localhost:7197/GetAllVoucher";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error fetching vouchers");
            }

            var apiData = await response.Content.ReadAsStringAsync();
            var lstVoucher = JsonConvert.DeserializeObject<List<Voucher>>(apiData);

            return lstVoucher.Any(v => v.Ten.Equals(voucherName, StringComparison.OrdinalIgnoreCase) && v.MaVoucher != currentVoucherId);
        }
    }

}
