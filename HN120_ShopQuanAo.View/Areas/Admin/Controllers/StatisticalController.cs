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
            var revenueResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe2/doanh-thu?day={today.Day}&month={today.Month}&year={today.Year}");
            var revenueJson = await revenueResponse.Content.ReadAsStringAsync();
            viewModel.TongDoanhThu = JsonConvert.DeserializeObject<decimal>(revenueJson);

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
            var topProductsResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe2/top-san-pham-ban-chay?day={today.Day}&month={today.Month}&year={today.Year}&top=10");
            var topProductsJson = await topProductsResponse.Content.ReadAsStringAsync();
            viewModel.TopSanPhamBanChay = JsonConvert.DeserializeObject<List<SanPhamBanChayViewModel>>(topProductsJson);

            return View(viewModel);
        }
    }
}
