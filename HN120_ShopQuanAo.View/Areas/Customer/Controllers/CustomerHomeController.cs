using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.View.Areas.Customer.Data;
using HN120_ShopQuanAo.View.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Newtonsoft.Json;
using System.Text;

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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ApiurlSanPham = "https://localhost:7197/api/SanPham/GetAllSanPham";
            var resposeSP = await _httpClient.GetAsync(ApiurlSanPham);
            string apidatasSP = await resposeSP.Content.ReadAsStringAsync();
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apidatasSP);
            if (lstSP != null)
            {
                var LstSP_ok = lstSP.Where(x => x.TrangThai == 1).ToList();
                var lstSPPagi = LstSP_ok.Take(9).ToList();
                ViewBag.ListSP = lstSPPagi;
                return View();
            }
            else
            {
                return BadRequest("Api lỗi data sản phẩm rồi");
            }
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
               var LstSP_ok = lstSP.Where(x => x.TrangThai == 1).ToList();
                ViewBag.ListSP = LstSP_ok;
                return View();
            }
            else
            {
                return BadRequest("Api lỗi data sản phẩm rồi");
            }
        }

        // Giỏ Hàng của Khách hàng
        [HttpGet]
        public async Task<IActionResult> GioHangCuaBan()
        {
            var maND = Request.Cookies["UserId"];
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
                if (GHCB == null)
                {
                    return BadRequest("Giỏ Hàng đang null kìa, xem lại api truyền vào");
                }


                // lấy danh sách màu sắc
                var urlMauSac = $"https://localhost:7197/api/MauSac/GetAllMauSac";
                var responMS = await _httpClient.GetAsync(urlMauSac);
                string apiDataMS = await responMS.Content.ReadAsStringAsync();
                var lstMauSac = JsonConvert.DeserializeObject<List<MauSac>>(apiDataMS);
                if (lstMauSac == null)
                {
                    TempData["MessageMauSacNull"] = "Sản phẩm này không có màu";
                }
                ViewBag.lstMauSac = lstMauSac;

                // Lấy List Size
                var urlSize = $"https://localhost:7197/api/Size/GetAllSize";
                var ResponSize = await _httpClient.GetAsync(urlSize);
                string apidataSize = await ResponSize.Content.ReadAsStringAsync();
                var lstSize = JsonConvert.DeserializeObject<List<Size>>(apidataSize);
                if (lstSize == null)
                {
                    TempData["MessageSizeNull"] = "Sản phẩm này đang không có Size";
                }
                ViewBag.ListSize = lstSize;

                // danh sách sản phẩm
                var urlSP = $"https://localhost:7197/api/SanPham/GetAllSanPham";
                var responSP = await _httpClient.GetAsync(urlSP);
                string apiDataSP = await responSP.Content.ReadAsStringAsync();
                var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apiDataSP);

                // danh sách chi tiết sản phẩm
                var apiCTSP = "https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
                var responCTSP = await _httpClient.GetAsync(apiCTSP);
                string apidataCTSP = await responCTSP.Content.ReadAsStringAsync();
                var lstCTSP = JsonConvert.DeserializeObject<List<ChiTietSp>>(apidataCTSP);

                var joinData = from sp in lstSP
                               join ctsp in lstCTSP on sp.MaSp equals ctsp.MaSp
                               join ghct in GHCB on ctsp.SKU equals ghct.SKU
                               join ms in lstMauSac on ctsp.MaMau equals ms.MaMau
                               join sz in lstSize on ctsp.MaSize equals sz.MaSize
                               where ctsp.SoLuongTon > 0
                               select new GioHangChiTietView
                               {
                                   MaGHCT = ghct.MaGioHangChiTiet,
                                   UrlAnhSPCT = ctsp.UrlAnhSpct,
                                   Masp = sp.MaSp,
                                   TenSP = sp.TenSP,
                                   SKU = ctsp.SKU,
                                   MaMau = ms.MaMau,
                                   MaSize = sz.MaSize,
                                   TenMau = ms.TenMau,
                                   TenSize = sz.TenSize,
                                   DonGia = ctsp.DonGia,
                                   SoLuong = ghct.SoLuong,
                                   ThanhTien = ctsp.DonGia * ghct.SoLuong
                               };
                var listGHCTview = joinData.ToList();
                ViewBag.JoinDataGH = listGHCTview;
                if (listGHCTview == null)
                {
                    return BadRequest("Ko lấy được data viewModel");
                }
                return View();
            }
            return BadRequest();
        }
        // Click để vào trang Detail của Sản phẩm
        ////[HttpGet]
        ////public async Task< IActionResult> TTSanPham(string maSP)
        ////{       
        ////    var ApiurllstGioHangcuaban = "https://localhost:7197/api/CTSanPham/GetCTSPById/{maSP}";
        ////    var responseListGHCB = await _httpClient.GetAsync(ApiurllstGioHangcuaban);
        ////    string apidataListGHCB = await responseListGHCB.Content.ReadAsStringAsync();
        ////    var SP = JsonConvert.DeserializeObject<SanPham>(apidataListGHCB);
        ////    if (SP != null)
        ////    {
        ////        ViewBag.SanPham = SP;
        ////        return RedirectToAction("TTSanPham","CustomerHome", new {areas = "Customer"});
        ////    }
        ////    return View();
        ////}
        [HttpPost]
        // Thêm sản phẩm 
        public async Task<IActionResult> AddToCart(string maSPCT, int soluong)
        {

            var maND = Request.Cookies["UserId"];
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
                if (GHCB == null)
                {
                    return BadRequest("Giỏ Hàng đang null kìa, xem lại api truyền vào");
                }
                var UrlSPCT = $"https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
                var responSPCT = await _httpClient.GetAsync(UrlSPCT);
                string apiDataSPCT = await responSPCT.Content.ReadAsStringAsync();
                var lstSPCT = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDataSPCT);
                var spct = lstSPCT.FirstOrDefault(x => x.MaSp == maSPCT);
                if(spct == null)
                {
                    return BadRequest("Lỗi truyền thông tin sản phẩm chi tiết, không lấy được thông tin");
                }

                var ApiurlSanPham = "https://localhost:7197/api/SanPham/GetAllSanPham";
                var resposeSP = await _httpClient.GetAsync(ApiurlSanPham);
                string apidatasSP = await resposeSP.Content.ReadAsStringAsync();
                var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apidatasSP);

                var sp = lstSP.FirstOrDefault(x => x.MaSp == spct.MaSp);



                GioHangChiTiet ghct = new GioHangChiTiet();
                ghct.MaGioHangChiTiet = Guid.NewGuid().ToString();
                ghct.MaGioHang = Ghcb.MaGioHang;
                ghct.SKU = spct.SKU;
                ghct.TenSp = sp.TenSP;
                ghct.DonGia = spct.DonGia;
                ghct.SoLuong = soluong;
                ghct.ThanhTien = spct.DonGia * soluong;
                ghct.TrangThai = 1;

                var urlGHCT = $"https://localhost:7197/api/GioHangChiTiet/CreateGioHangChiTiet?MaGH={ghct.MaGioHang}&SKU={ghct.SKU}&TenSP={ghct.TenSp}&DonGia={ghct.DonGia}&SoLuong={ghct.SoLuong}&ThanhTien={ghct.ThanhTien}&TrangThai={ghct.TrangThai}";
                var httpClient = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(ghct), Encoding.UTF8, "application/json");
                var respon = await httpClient.PostAsync(urlGHCT, content);
                if (respon.IsSuccessStatusCode)
                {
                    return RedirectToAction("GioHangCuaBan","CustomerHome", new { Areas = "Customer"});
                }

            }
                // lấy danh sách màu sắc
               
            return BadRequest();
        }
        //Detail Sản phẩm
        [HttpGet]
        public async Task<IActionResult> DetailSanPham(string masp)
        {
            // lấy danh sách sản phẩm
            var ApiurlSanPham = "https://localhost:7197/api/SanPham/GetAllSanPham";
            var resposeSP = await _httpClient.GetAsync(ApiurlSanPham);
            string apidatasSP = await resposeSP.Content.ReadAsStringAsync();
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apidatasSP);
            if(lstSP == null)
            {
                TempData["MessageSPNull"] = "Không lấy được dữ liệu sản phẩm";
                return View(); 
            }
            // lấy danh sách sản phẩm có trạng thái khả dụng
            var lstSP_ok = lstSP.Where(x => x.TrangThai == 1).ToList();
            if (lstSP_ok == null)
            {
                TempData["MessageSPNull"] = "Không lấy được dữ liệu sản phẩm khả dụng";
                return View();
            }
            // list sản phẩm có thể order thêm ở bên dưới (không quan trọng)
            var lstSP_CanOrder = lstSP_ok.OrderBy(x => x.NgayNhap).Take(5);
            ViewBag.lstSP_CanOrder = lstSP_CanOrder;
            //lấy sản phẩm Detail
            var SP = lstSP_ok.FirstOrDefault(x => x.MaSp == masp);
            if (SP == null)
            {
                TempData["MessageSPNull"] = "Không lấy được dữ liệu sản phẩm cần mua";
                return View();
            }
            ViewBag.SP = SP;

            // Lấy toàn bộ danh sách CTSP
            var urlSPCT = $"https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
            var responBook = await _httpClient.GetAsync(urlSPCT);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDataBook);
            if(lstBook == null)
            {
                TempData["Error Message"] = "Sản Phẩm không khả dụng";
            }
            //Lấy danh sách sản phẩm chi tiết thông qua mã sản phâm

            var ListCTSP = lstBook.Where(x => x.MaSp == SP.MaSp).ToList();
            if(ListCTSP == null)
            {
                TempData["MessageSPCTNull"] = "Sản Phẩm hiện không khả dụng";
            }
            ViewBag.ListCTSP = ListCTSP;
            // Lấy List Size
            var urlSize = $"https://localhost:7197/api/Size/GetAllSize";
            //var httpClient = new HttpClient();
            var ResponSize = await _httpClient.GetAsync(urlSize);
            string apidataSize = await ResponSize.Content.ReadAsStringAsync();
            var lstSize = JsonConvert.DeserializeObject<List<Size>>(apidataSize);
            if(lstSize == null)
            {
                TempData["MessageSizeNull"] = "Sản phẩm này đang không có Size";
            }
            ViewBag.ListSize = lstSize;
            // Lấy List Màu Sắc
            var urlMauSac = $"https://localhost:7197/api/MauSac/GetAllMauSac";
            //var httpClient = new HttpClient();
            var responMS = await _httpClient.GetAsync(urlMauSac);
            string apiDataMS = await responMS.Content.ReadAsStringAsync();
            var lstMauSac = JsonConvert.DeserializeObject<List<MauSac>>(apiDataMS);
           if(lstMauSac == null)
            {
                TempData["MessageMauSacNull"] = "Sản phẩm này không có màu";
            }
           ViewBag.lstMauSac = lstMauSac;
            return View();
           
        }
    }
}
