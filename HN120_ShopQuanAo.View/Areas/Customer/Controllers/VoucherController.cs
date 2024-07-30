using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.View.Areas.Customer.Data;
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
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> layvouchervekho(string mavc)
        {
            var maND = Request.Cookies["UserId"];


            var urlvc = $"https://localhost:7197/GetAllVoucher";
            var responseVC = await _httpClient.GetAsync(urlvc);
            string apiurlVC = await responseVC.Content.ReadAsStringAsync();
            var lstVC = JsonConvert.DeserializeObject<List<Voucher>>(apiurlVC);

            var urlUVC = $"https://localhost:7197/api/Voucher_User/GetVoucher_UserbyUserId/{maND}";
            var responseUVC = await _httpClient.GetAsync(urlUVC);
            string apiurlUVC = await responseUVC.Content.ReadAsStringAsync();
            var lstUVC = JsonConvert.DeserializeObject<List<User_Voucher>>(apiurlUVC);
            var joinDataUVC = from vc in lstVC
                              join uvc in lstUVC on vc.MaVoucher equals uvc.MaVoucher
                              select new VoucherUserView
                              {
                                  UserVoucherId = uvc.UserVoucherID,
                                  UserId = maND,
                                  MaVoucher = vc.MaVoucher,
                                  TenVoucher = vc.Ten,
                                  DonGiaToiThieu = vc.GiaGiamToiThieu,
                                  GiamGiaToiDa = vc.GiaGiamToiDa,
                                  GiaTriGiam = vc.GiaTriGiam,
                                  TrangThai = uvc.TrangThai
                              };
            ViewBag.lstUVC = joinDataUVC.ToList();

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
