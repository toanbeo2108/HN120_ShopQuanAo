using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.View.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Newtonsoft.Json;

namespace HN120_ShopQuanAo.View.Areas.Customer.Controllers
{
    public class CustomerHomeController : Controller
    {
        private readonly ILogger<CustomerHomeController> _logger;
        private readonly HttpClient _httpClient;

        public CustomerHomeController(ILogger<CustomerHomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }
        //Gian Hàng của khách hàng
        [HttpGet]
        public async Task<IActionResult> GianHangNguoiDung()
        {
            var ApiurlSanPham = "https://localhost:7197/api/SanPham/GetAllSanPham";
            var resposeSP = await _httpClient.GetAsync(ApiurlSanPham);
            string apidatasSP = await resposeSP.Content.ReadAsStringAsync();
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apidatasSP);
            if(lstSP != null)
            {
                ViewBag.ListSP = lstSP;
                return View();
            }
            else
            {
                return BadRequest("Api lỗi data sản phẩm rồi");
            }
        }

        // Giỏ Hàng của Khách hàng
        [HttpGet]
        public async Task<IActionResult> GioHangCuaBan(string maND)
        {
            var ApiurllstGioHangcuaban = "https://localhost:7197/api/GioHang/GetGHByUserId/{maND}";
            var responseListGHCB = await _httpClient.GetAsync(ApiurllstGioHangcuaban);
            string apidataListGHCB = await responseListGHCB.Content.ReadAsStringAsync();
            var ListGHCB = JsonConvert.DeserializeObject<List<GioHang>>(apidataListGHCB);
            if(ListGHCB != null)
            {
                var Ghcb = ListGHCB.FirstOrDefault(x => x.TrangThai == 1);
                if(Ghcb == null)
                {
                    return BadRequest("Không có Mã ND nên Null là đúng");
                }
                var ApiurlGioHangcuaban = "https://localhost:7197/api/GioHangChiTiet/GetGHCTByMaGH/{Ghcb.MaGioHang}";
                var responseGHCB = await _httpClient.GetAsync(ApiurlGioHangcuaban);
                string apidataGHCB = await responseGHCB.Content.ReadAsStringAsync();
                var GHCB = JsonConvert.DeserializeObject<List<GioHangChiTiet>>(apidataGHCB);
                if(GHCB != null)
                {
                    ViewBag.GHCB = GHCB;
                    return View();
                }
                return View();
            }
            return BadRequest();
        }
        // Click để vào trang Detail của Sản phẩm
        [HttpGet]
        public async Task< IActionResult> TTSanPham(string maSP)
        {       
            var ApiurllstGioHangcuaban = "https://localhost:7197/api/CTSanPham/GetCTSPById/abc{maSP}";
            var responseListGHCB = await _httpClient.GetAsync(ApiurllstGioHangcuaban);
            string apidataListGHCB = await responseListGHCB.Content.ReadAsStringAsync();
            var SP = JsonConvert.DeserializeObject<SanPham>(apidataListGHCB);
            if (SP != null)
            {
                ViewBag.SanPham = SP;
                return RedirectToAction("TTSanPham","CustomerHome", new {areas = "Customer"});
            }
            return View();
        }
        [HttpPost]
        // Thêm sản phẩm 
        public async Task<IActionResult> AddToCart(string maND,string maSP)
        {
            var ApiurllstGioHangcuaban = "https://localhost:7197/api/GioHang/GetGHByUserId/{maND}";
            var responseListGHCB = await _httpClient.GetAsync(ApiurllstGioHangcuaban);
            string apidataListGHCB = await responseListGHCB.Content.ReadAsStringAsync();
            var ListGHCB = JsonConvert.DeserializeObject<List<GioHang>>(apidataListGHCB);
            if (ListGHCB != null)
            {
                var Ghcb = ListGHCB.FirstOrDefault(x => x.TrangThai == 1);
                if (Ghcb == null)
                {
                    return BadRequest("Không có Mã ND nên Null là đúng");
                }
                var ApiurlGioHangcuaban = "https://localhost:7197/api/GioHangChiTiet/GetGHCTByMaGH/{Ghcb.MaGioHang}";
                var responseGHCB = await _httpClient.GetAsync(ApiurlGioHangcuaban);
                string apidataGHCB = await responseGHCB.Content.ReadAsStringAsync();
                var GHCB = JsonConvert.DeserializeObject<List<GioHangChiTiet>>(apidataGHCB);
                if (GHCB != null)
                {
                    
                    return View();
                }
                return View();
            }
            return BadRequest();
        }
    }
}
