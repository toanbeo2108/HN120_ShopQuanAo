using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.View.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        private readonly HttpClient _httpClient;
        public StatisticalController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
       
        public async Task<IActionResult> ThongKe()
        {
            var viewModel = new ThongKeViewModel();

            // Lấy dữ liệu doanh thu ngày hôm nay
            var today = DateTime.Today;
            var sevenDaysAgo = today.AddDays(-7);

            var revenueResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe2/doanh-thu?day={today.Day}&month={today.Month}&year={today.Year}");
            var revenueJson = await revenueResponse.Content.ReadAsStringAsync();
            viewModel.TongDoanhThu = JsonConvert.DeserializeObject<decimal>(revenueJson);

             // Lấy dữ liệu doanh thu theo ngày cho 7 ngày gần nhất
            var revenueDailyResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe2/doanh-thu-theo-ngay?fromDate={sevenDaysAgo:yyyy-MM-dd}&toDate={today:yyyy-MM-dd}");
            var revenueDailyJson = await revenueDailyResponse.Content.ReadAsStringAsync();
            viewModel.DoanhThuTheoNgay = JsonConvert.DeserializeObject<List<DoanhThuViewModel>>(revenueDailyJson);

            // Lấy số lượng sản phẩm bán được ngày hôm nay
            var productCountResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe2/so-luong-san-pham?day={today.Day}&month={today.Month}&year={today.Year}");
            var productCountJson = await productCountResponse.Content.ReadAsStringAsync();
            viewModel.SoLuongSanPham = JsonConvert.DeserializeObject<int>(productCountJson);

            // Lấy số lượng hóa đơn bán được ngày hôm nay
            var invoiceCountResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe2/so-luong-hoa-don?day={today.Day}&month={today.Month}&year={today.Year}");
            var invoiceCountJson = await invoiceCountResponse.Content.ReadAsStringAsync();
            viewModel.SoLuongHoaDon = JsonConvert.DeserializeObject<int>(invoiceCountJson);

            // Lấy danh sách sản phẩm sắp hết hàng
            var lowStockProductsResponse = await _httpClient.GetAsync("https://localhost:7197/api/ThongKe2/san-pham-sap-het-hang");
            var lowStockProductsJson = await lowStockProductsResponse.Content.ReadAsStringAsync();
            viewModel.SanPhamSapHetHang = JsonConvert.DeserializeObject<List<SanPhamSapHetHangViewModel>>(lowStockProductsJson);

            // Lấy danh sách top sản phẩm bán chạy ngày hôm nay
            var topProductsResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe2/top-selling-product2?day={today.Day}&month={today.Month}&year={today.Year}&top=10");
            var topProductsJson = await topProductsResponse.Content.ReadAsStringAsync();
            viewModel.TopSanPhamBanChay = JsonConvert.DeserializeObject<List<SanPhamBanChayViewModel>>(topProductsJson);


            // Lấy danh sách sản phẩm
            var ListSanPhamReponse = await _httpClient.GetAsync("https://localhost:7197/api/SanPham/GetAllSanPham");
            var ListSanPhamJson = await topProductsResponse.Content.ReadAsStringAsync();
            ViewBag.ListSanPham = JsonConvert.DeserializeObject<List<SanPham>>(ListSanPhamJson);

            // Lấy danh sách chi tiết sản phẩm
            var ListCTSanPhamReponse = await _httpClient.GetAsync("https://localhost:7197/api/CTSanPham/GetAllCTSanPham");
            var ListCTSanPhamJson = await ListCTSanPhamReponse.Content.ReadAsStringAsync();
            ViewBag.ListCTSanPham = JsonConvert.DeserializeObject<List<ChiTietSp>>(ListCTSanPhamJson);


            return View(viewModel);
        }


        [HttpGet]
        public IActionResult ThongKeChiTiet()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ThongKeChiTiet(DateTime? fromDate, DateTime? toDate)
        {
            var viewModel = new ThongKeViewModel();

            if (fromDate.HasValue && toDate.HasValue)
            {
                // Lấy dữ liệu doanh thu theo ngày cho khoảng thời gian được chọn
                var revenueDailyResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe2/doanh-thu-theo-ngay?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}");
                var revenueDailyJson = await revenueDailyResponse.Content.ReadAsStringAsync();
                viewModel.DoanhThuTheoNgay = JsonConvert.DeserializeObject<List<DoanhThuViewModel>>(revenueDailyJson);

                // Lấy số lượng sản phẩm bán được trong khoảng thời gian được chọn
                var productCountResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe2/so-luong-san-pham?startDate={fromDate:yyyy-MM-dd}&endDate={toDate:yyyy-MM-dd}");
                var productCountJson = await productCountResponse.Content.ReadAsStringAsync();
                viewModel.SoLuongSanPham = JsonConvert.DeserializeObject<int>(productCountJson);

                // Lấy số lượng hóa đơn bán được trong khoảng thời gian được chọn
                var invoiceCountResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe2/so-luong-hoa-don?startDate={fromDate:yyyy-MM-dd}&endDate={toDate:yyyy-MM-dd}");
                var invoiceCountJson = await invoiceCountResponse.Content.ReadAsStringAsync();
                viewModel.SoLuongHoaDon = JsonConvert.DeserializeObject<int>(invoiceCountJson);

                // Lấy danh sách top sản phẩm bán chạy trong khoảng thời gian được chọn
                var topProductsResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe2/top-selling-product2?startDate={fromDate:yyyy-MM-dd}&endDate={toDate:yyyy-MM-dd}");
                var topProductsJson = await topProductsResponse.Content.ReadAsStringAsync();
                viewModel.TopSanPhamBanChay = JsonConvert.DeserializeObject<List<SanPhamBanChayViewModel>>(topProductsJson);
            }

            return View(viewModel);
        }

    }
}
