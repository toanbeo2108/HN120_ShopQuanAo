using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class LichSuVoucherController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LichSuVoucherController> _logger;
        private readonly AppDbContext _context;

        public LichSuVoucherController(ILogger<LichSuVoucherController> logger)
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
        public async Task<IActionResult> AllVoucherHManager()
        {
            try
            {
                var urlBook = $"https://localhost:7197/GetAllVoucher";
                var responseBook = await _httpClient.GetAsync(urlBook);

                if (responseBook.IsSuccessStatusCode)
                {
                    string apiDataBook = await responseBook.Content.ReadAsStringAsync();
                    var lstBook = JsonConvert.DeserializeObject<List<VoucherHistory>>(apiDataBook);

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

                    //lstBook = lstBook.OrderByDescending(v => v.MaVoucher).ToList();
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
    }
}
