using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Customer.Controllers
{
    public class VoucherController : Controller
    {
        private HttpClient _httpClient;
        public VoucherController()
        {
            _httpClient = new HttpClient();
        }
        [HttpGet]
        public async Task<IActionResult> khuyenmai()
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
                    ViewBag.lstVoucher = lstBook;
                    return View();
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
        [HttpPost]
        public async Task<IActionResult> layvouchervekho(string mavc)
        {
            var maND = Request.Cookies["UserId"];
            var urlcreateUVC = $"https://localhost:7197/api/Voucher_User/CreateUVC?userid={maND}&mavc={mavc}";
            var content = new StringContent("Thêm thành công", Encoding.UTF8, "application/json");
            var respon = await _httpClient.PostAsync(urlcreateUVC, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("khuyenmai", "Voucher", new { areas = "Customer" });
            }
            return BadRequest("Không thể lấy vouher, lỗi rồi");

        }
    }
}
