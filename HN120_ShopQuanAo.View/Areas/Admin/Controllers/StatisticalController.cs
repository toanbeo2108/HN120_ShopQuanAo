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
        public async Task<IActionResult> GetDoanhThu7NgayGanNhat()
        {
            var response = await _httpClient.GetAsync("https://localhost:7197/api/ThongKe/ThongKeDoanhThu7NgayGanNhat");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var doanhThuList = JsonConvert.DeserializeObject<List<DoanhThuViewModel>>(jsonResponse);

            var tongDoanhThuResponse = await _httpClient.GetAsync("https://localhost:7197/api/ThongKe/TongDoanhThu");
            tongDoanhThuResponse.EnsureSuccessStatusCode();
            var tongDoanhThu = await tongDoanhThuResponse.Content.ReadAsStringAsync();

            var tongSanPhamBanDuocResponse = await _httpClient.GetAsync("https://localhost:7197/api/ThongKe/TongSanPhamBanDuoc");
            tongSanPhamBanDuocResponse.EnsureSuccessStatusCode();
            var tongSanPhamBanDuoc = await tongSanPhamBanDuocResponse.Content.ReadAsStringAsync();

            var tongSoLuongHoaDonResponse = await _httpClient.GetAsync("https://localhost:7197/api/ThongKe/TongSoLuongHoaDon");
            tongSoLuongHoaDonResponse.EnsureSuccessStatusCode();
            var tongSoLuongHoaDon = await tongSoLuongHoaDonResponse.Content.ReadAsStringAsync();

            ViewBag.TongDoanhThu = tongDoanhThu;
            ViewBag.TongSanPhamBanDuoc = tongSanPhamBanDuoc;
            ViewBag.TongSoLuongHoaDon = tongSoLuongHoaDon;

            return View(doanhThuList);
        }
        public async Task<IActionResult> ChiTietThongKe(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate == null || toDate == null)
            {
                return View(new List<DoanhThuViewModel>());
            }

            var doanhThuResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe/ThongKeDoanhThu?fromDate={fromDate}&toDate={toDate}");
            doanhThuResponse.EnsureSuccessStatusCode();
            var doanhThuJsonResponse = await doanhThuResponse.Content.ReadAsStringAsync();
            var doanhThuObj = JsonConvert.DeserializeObject<dynamic>(doanhThuJsonResponse);
            decimal tongDoanhThu = doanhThuObj?.tongDoanhThu ?? 0;

            var doanhThuTheoNgayResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe/ThongKeDoanhThuTheoNgay?fromDate={fromDate}&toDate={toDate}");
            doanhThuTheoNgayResponse.EnsureSuccessStatusCode();
            var doanhThuTheoNgayJsonResponse = await doanhThuTheoNgayResponse.Content.ReadAsStringAsync();
            var doanhThuTheoNgayList = JsonConvert.DeserializeObject<List<DoanhThuViewModel>>(doanhThuTheoNgayJsonResponse);

            var hoaDonResponse = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe/ThongKeHoaDon?fromDate={fromDate}&toDate={toDate}");
            hoaDonResponse.EnsureSuccessStatusCode();
            var hoaDonJsonResponse = await hoaDonResponse.Content.ReadAsStringAsync();
            var hoaDonObj = JsonConvert.DeserializeObject<dynamic>(hoaDonJsonResponse);
            int soLuongHoaDon = hoaDonObj?.soLuongHoaDon ?? 0;

            ViewBag.TongDoanhThu = tongDoanhThu;
            ViewBag.SoLuongSanPham = doanhThuTheoNgayList.Sum(x => x.SoLuongSanPham);
            ViewBag.SoLuongHoaDon = soLuongHoaDon;

            ViewBag.FromDate = fromDate.Value.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate.Value.ToString("yyyy-MM-dd");

            return View(doanhThuTheoNgayList);
        }

        public async Task<List<DoanhThuViewModel>> GetDoanhThuTheoNgay(DateTime? fromDate, DateTime? toDate)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7197/api/ThongKe/ThongKeDoanhThuTheoNgay?fromDate={fromDate}&toDate={toDate}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DoanhThuViewModel>>(jsonResponse);
        }
    }
}
