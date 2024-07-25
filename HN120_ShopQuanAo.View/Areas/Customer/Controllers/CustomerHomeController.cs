using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using HN120_ShopQuanAo.View.Areas.Admin.Data;
using HN120_ShopQuanAo.View.Areas.Customer.Data;
using HN120_ShopQuanAo.View.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;


namespace HN120_ShopQuanAo.View.Areas.Customer.Controllers
{
    public class CustomerHomeController : Controller
    {
        private readonly ILogger<CustomerHomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _appDbContext;
        string Sku { get; set; }
        int sl { get; set; }
        public CustomerHomeController(ILogger<CustomerHomeController> logger)
        {
            _appDbContext = new AppDbContext();
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
            var ApiurlSanPham = $"https://localhost:7197/api/SanPham/GetAllSanPham";
            var resposeSP = await _httpClient.GetAsync(ApiurlSanPham);
            string apidatasSP = await resposeSP.Content.ReadAsStringAsync();
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apidatasSP);
            if (lstSP != null)
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
            decimal? tienship = 0;
            decimal? tongtien = 0;
            var maND = Request.Cookies["UserId"];
            var ApiurllstGioHangcuaban = $"https://localhost:7197/api/GioHang/GetGHByUserId/{maND}";
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
                var ApiurlGioHangcuaban = $"https://localhost:7197/api/GioHangChiTiet/GetGHCTByMaGH/{Ghcb.MaGioHang}";
                var responseGHCB = await _httpClient.GetAsync(ApiurlGioHangcuaban);
                string apidataGHCB = await responseGHCB.Content.ReadAsStringAsync();
                var GHCB = JsonConvert.DeserializeObject<List<GioHangChiTiet>>(apidataGHCB);
                if (GHCB == null)
                {
                    return BadRequest("Giỏ Hàng đang null kìa, xem lại api truyền vào");
                }
                // tính lại tổng tiền 
                foreach (var item in GHCB)
                {
                    tongtien += item.ThanhTien;
                }
                Ghcb.TongTien = tongtien;
                // update thông tin giỏ hàng
                var urlupdateGH = $"https://localhost:7197/api/GioHang/UpdateGH/{Ghcb.MaGioHang}?tongtien={Ghcb.TongTien}&trangthai=1";
                var content = new StringContent("Cập nhật thành công");

                var responseUpdateGH = await _httpClient.PutAsync(urlupdateGH, content);

                decimal? tiencantra = tongtien + tienship;
                ViewBag.TongTien = tongtien;
                ViewBag.TienShip = tienship;
                ViewBag.TìenCanTra = tiencantra;


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
        public async Task<IActionResult> AddToCart(
            GioHangChiTiet ghct)
        {

            var maND = Request.Cookies["UserId"];
            var ApiurllstGioHangcuaban = $"https://localhost:7197/api/GioHang/GetGHByUserId/{maND}";
            var responseListGHCB = await _httpClient.GetAsync(ApiurllstGioHangcuaban);
            string apidataListGHCB = await responseListGHCB.Content.ReadAsStringAsync();
            var ListGHCB = JsonConvert.DeserializeObject<List<GioHang>>(apidataListGHCB);
            if (ListGHCB == null)
            {
                return BadRequest("Giỏ Hàng của bạn không tồn tại");

            }
            var Ghcb = ListGHCB.FirstOrDefault(x => x.TrangThai == 1);
            if (Ghcb == null)
            {
                return BadRequest("Không có Mã ND nên Null là đúng");
            }

            var ApiurlGioHangcuaban = $"https://localhost:7197/api/GioHangChiTiet/GetGHCTByMaGH/{Ghcb.MaGioHang}";
            var responseGHCB = await _httpClient.GetAsync(ApiurlGioHangcuaban);
            string apidataGHCB = await responseGHCB.Content.ReadAsStringAsync();
            var GHCB = JsonConvert.DeserializeObject<List<GioHangChiTiet>>(apidataGHCB);
            if (GHCB == null)
            {
                return BadRequest("Giỏ Hàng không lấy được thông tin người dùng, yêu cầu đăng nhập lại");
            }
            foreach (var item in GHCB)
            {
                if (ghct.SKU == item.SKU)
                {
                    item.SoLuong += ghct.SoLuong;
                    var urlGHCTUpdate = $"https://localhost:7197/api/GioHangChiTiet/UpdateGHCT/{item.MaGioHangChiTiet}?soluong={item.SoLuong}";
                    var contentUpdate = new StringContent("Update thành công");
                    var responseUpdate = await _httpClient.PutAsync(urlGHCTUpdate, contentUpdate);
                    if (responseUpdate.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GioHangCuaBan", "CustomerHome", new { Areas = "Customer" });
                    }
                }
            }
            var ApiurlSanPham = "https://localhost:7197/api/SanPham/GetAllSanPham";
            var resposeSP = await _httpClient.GetAsync(ApiurlSanPham);
            string apidatasSP = await resposeSP.Content.ReadAsStringAsync();
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apidatasSP);

            if (ghct.TenSp == null || ghct.DonGia == null || ghct.SoLuong == null || ghct.ThanhTien == null || ghct.TrangThai == null)
            {
                return RedirectToAction("GioHangCuaBan", "CustomerHome", new { Areas = "Customer" });
            }
            var urlGHCT = $"https://localhost:7197/api/GioHangChiTiet/CreateGioHangChiTiet?MaGH={Ghcb.MaGioHang}&SKU={ghct.SKU}&TenSP={ghct.TenSp}&DonGia={ghct.DonGia}&SoLuong={ghct.SoLuong}&ThanhTien={ghct.ThanhTien}&TrangThai={ghct.TrangThai}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(ghct), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlGHCT, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("GioHangCuaBan", "CustomerHome", new { Areas = "Customer" });
            }
            else
            {
                return BadRequest("Thêm thất bại");
            }




        }
        //Detail Sản phẩm
        [HttpGet]
        public async Task<IActionResult> DetailSanPham(string masp)
        {
            // lấy danh sách sản phẩm
            var ApiurlSanPham = $"https://localhost:7197/api/SanPham/GetAllSanPham";
            var resposeSP = await _httpClient.GetAsync(ApiurlSanPham);
            string apidatasSP = await resposeSP.Content.ReadAsStringAsync();
            var lstSP = JsonConvert.DeserializeObject<List<SanPham>>(apidatasSP);
            if (lstSP == null)
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
            if (lstBook == null)
            {
                TempData["Error Message"] = "Sản Phẩm không khả dụng";
            }
            //Lấy danh sách sản phẩm chi tiết thông qua mã sản phâm

            var ListCTSP = lstBook.Where(x => x.MaSp == SP.MaSp).ToList();
            if (ListCTSP == null)
            {
                TempData["MessageSPCTNull"] = "Sản Phẩm hiện không khả dụng";
            }
            ViewBag.ListCTSP = ListCTSP;
            var minCTSP = ListCTSP.OrderBy(x => x.GiaBan).FirstOrDefault();
            ViewBag.minCTSP = minCTSP;
            var maxCTSP = ListCTSP.OrderBy(x => x.GiaBan).LastOrDefault();
            ViewBag.maxCTSP = maxCTSP;
            // Lấy List Size
            var urlSize = $"https://localhost:7197/api/Size/GetAllSize";
            //var httpClient = new HttpClient();
            var ResponSize = await _httpClient.GetAsync(urlSize);
            string apidataSize = await ResponSize.Content.ReadAsStringAsync();
            var lstSize = JsonConvert.DeserializeObject<List<Size>>(apidataSize);
            if (lstSize == null)
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
            if (lstMauSac == null)
            {
                TempData["MessageMauSacNull"] = "Sản phẩm này không có màu";
            }
            ViewBag.lstMauSac = lstMauSac;


            var maND = Request.Cookies["UserId"];
            var ApiurllstGioHangcuaban = $"https://localhost:7197/api/GioHang/GetGHByUserId/{maND}";
            var responseListGHCB = await _httpClient.GetAsync(ApiurllstGioHangcuaban);
            string apidataListGHCB = await responseListGHCB.Content.ReadAsStringAsync();
            var ListGHCB = JsonConvert.DeserializeObject<List<GioHang>>(apidataListGHCB);
            if (ListGHCB == null)
            {
                return BadRequest("Giỏ Hàng của bạn không tồn tại");

            }
            var Ghcb = ListGHCB.FirstOrDefault(x => x.TrangThai == 1);
            if (Ghcb == null)
            {
                return BadRequest("Không có Mã ND nên Null là đúng");
            }
            ViewBag.MaGH = Ghcb.MaGioHang;
            return View();

        }
        public void DeleteItemGioHang(List<GioHangChiTiet> lstSP)
        {
            foreach (var item in lstSP)
            {
                lstSP.Remove(item);
            }
        }
        // Xóa sẩn phẩm khỏi giỏ hàng
        [HttpGet]
        public async Task<IActionResult> XoaSPGioHang(string maSP)
        {


            var url = $"https://localhost:7197/api/GioHangChiTiet/DeleteGHCT/{maSP}";
            var respon = await _httpClient.DeleteAsync(url);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("GioHangCuaBan", "CustomerHome", new { Areas = "Customer" });
            }
            return BadRequest("Không thể xóa sản phẩm này khỏi giỏ hàng, vui long kiểm tra lại");

            //https://localhost:7197/api/GioHangChiTiet/DeleteGHCT/1
        }
        [HttpPost]
        public async Task<IActionResult> UpdateGioHang()
        {
            // Lấy thông tin danh sách giỏ hàng từ User
            var maND = Request.Cookies["UserId"];
            var ApiurllstGioHangcuaban = $"https://localhost:7197/api/GioHang/GetGHByUserId/{maND}";
            var responseListGHCB = await _httpClient.GetAsync(ApiurllstGioHangcuaban);
            string apidataListGHCB = await responseListGHCB.Content.ReadAsStringAsync();
            var ListGHCB = JsonConvert.DeserializeObject<List<GioHang>>(apidataListGHCB);
            if (ListGHCB == null)
            {
                return BadRequest("Không tìm thấy thông tin giỏ hàng của bạn, vui lòng đăng nhập lại");
            }
            var ghcb = ListGHCB.FirstOrDefault(x => x.TrangThai == 1);
            if (ghcb == null)
            {
                return BadRequest("Giỏ Hàng của bạn đã được thanh toán");
            }
            // lấy cả item User và tình tông tiền
            decimal? tongtien = 0;
            var ApiurlGioHangcuaban = $"https://localhost:7197/api/GioHangChiTiet/GetGHCTByMaGH/{ghcb.MaGioHang}";
            var responseGHCB = await _httpClient.GetAsync(ApiurlGioHangcuaban);
            string apidataGHCB = await responseGHCB.Content.ReadAsStringAsync();
            var GHCB = JsonConvert.DeserializeObject<List<GioHangChiTiet>>(apidataGHCB);
            if (GHCB == null)
            {
                return BadRequest("Giỏ Hàng đang null kìa, xem lại api truyền vào");
            }
            foreach (var item in GHCB)
            {
                tongtien += item.ThanhTien;
            }
            var newGH = new GioHang
            {
                MaGioHang = ghcb.MaGioHang,
                TongTien = tongtien,
                MoTa = 0,
                TrangThai = 1,
            };
            // update thông tin giỏ hàng
            var urlupdateGH = $"https://localhost:7197/api/GioHang/UpdateGH/{ghcb.MaGioHang}?tongtien={tongtien}&trangthai=1";
            var content = new StringContent("Cập nhật thành công");

            var responseUpdateGH = await _httpClient.PutAsync(urlupdateGH, content);
            if (responseUpdateGH.IsSuccessStatusCode)
            {
                return RedirectToAction("GioHangCuaBan", "CustomerHome", new { areas = "Customer" });

            }
            return BadRequest("Không thẻ update thông tin giỏ hàng");
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            decimal? tongtien = 0;
            decimal? tonghoadon = 0;
            decimal? tienship = 0;
            var maND = Request.Cookies["UserId"];
            var ApiurllstGioHangcuaban = $"https://localhost:7197/api/GioHang/GetGHByUserId/{maND}";
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

               
                var ApiurlGioHangcuaban = $"https://localhost:7197/api/GioHangChiTiet/GetGHCTByMaGH/{Ghcb.MaGioHang}";
                var responseGHCB = await _httpClient.GetAsync(ApiurlGioHangcuaban);
                string apidataGHCB = await responseGHCB.Content.ReadAsStringAsync();
                var GHCB = JsonConvert.DeserializeObject<List<GioHangChiTiet>>(apidataGHCB);
                if (GHCB == null)
                {
                    return BadRequest("Không tìm thấy thông tin giỏ hàng");
                }
                foreach (var item in GHCB)
                {
                    tongtien += item.ThanhTien;
                }
                ViewBag.TongTienHang = tongtien;
                tonghoadon = tongtien + tienship;
                ViewBag.TongHoaDon = tonghoadon;
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
            }
           return  View();
        }

        //private dynamic GenerateRandomString(Random r, int v)
        //{
        //    throw new NotImplementedException();
        //}
        [HttpPost]
        public async Task<IActionResult> DatHang(decimal tienship, string tinh, string huyen, string xa, string cuthe, decimal tongtiendonhang )
        {
           if(tongtiendonhang == 0)
            {
                return RedirectToAction("GianHangNguoiDung", "CustomerHome", new { areas = "Customer" });
            }

            //Lấy thông tin người dùng
            var maND = Request.Cookies["UserId"];
            var urlND = $"https://localhost:7197/api/User/GetUserById?id={maND}";
            var responseND = await _httpClient.GetAsync(urlND);
            string apidataND = await responseND.Content.ReadAsStringAsync();
            var nd = JsonConvert.DeserializeObject<User>(apidataND);
            if(nd == null)
            {
                return BadRequest("Không tìm thấy thông tin của bạn, vui lòng đăng nhập lại");
            }

            //var apiurlgethdbyuserid = $"https://localhost:7197/api/HoaDon/GetAllHDByUserId/UserId?UserId={maND}";
            //var responsegethdbyuserid = await _httpClient.GetAsync(apiurlgethdbyuserid);
            //string apidatahdbyuserid = await responsegethdbyuserid.Content.ReadAsStringAsync();
            //var lstHDByUserId = JsonConvert.DeserializeObject<List<HoaDon>>(apidatahdbyuserid);
            //if(lstHDByUserId == null)
            //{
            //    return BadRequest("Không tìm thấy thông tin người dùng");
            //}
            // lấy danh sách giỏ hàng
            var ApiurllstGioHangcuaban = $"https://localhost:7197/api/GioHang/GetGHByUserId/{maND}";
            var responseListGHCB = await _httpClient.GetAsync(ApiurllstGioHangcuaban);
            string apidataListGHCB = await responseListGHCB.Content.ReadAsStringAsync();
            var ListGHCB = JsonConvert.DeserializeObject<List<GioHang>>(apidataListGHCB);
            if (ListGHCB == null)
            {
                return BadRequest("KHông tìm thấy thông tin giỏ hàng, vui lòng đăng nhập lại");
            }
            // lấy giỏ hàng chờ xử lý
            var Ghcb = ListGHCB.FirstOrDefault(x => x.TrangThai == 1);
            if (Ghcb == null)
            {
                return BadRequest("Không có Mã ND nên Null là đúng");
            }
            // lấy danh sách sản phẩm có trong giỏ hàng
            var ApiurlGioHangcuaban = $"https://localhost:7197/api/GioHangChiTiet/GetGHCTByMaGH/{Ghcb.MaGioHang}";
            var responseGHCB = await _httpClient.GetAsync(ApiurlGioHangcuaban);
            string apidataGHCB = await responseGHCB.Content.ReadAsStringAsync();
            var GHCB = JsonConvert.DeserializeObject<List<GioHangChiTiet>>(apidataGHCB);
            if (GHCB == null)
            {
                return BadRequest("Giỏ Hàng đang null kìa, xem lại api truyền vào");
            }
            // tạo sẵn 1 hóa đơn đẩy dữ liệu giỏ hàng vào
            var apiurlTaoHDUserId = $"https://localhost:7197/api/HoaDon/CreateHoaDonUser?UserId={maND}";
            var content = new StringContent("Tạo Hóa Đơn Thành Công");
            var respon = await _httpClient.PostAsync(apiurlTaoHDUserId, content);
            if (respon.IsSuccessStatusCode)
            {
                // lấy thông tin giỏ hàng vừa tạo 
                var apiurlgethdbyuserid = $"https://localhost:7197/api/HoaDon/GetAllHDByUserId/UserId?UserId={maND}";
                var responsegethdbyuserid = await _httpClient.GetAsync(apiurlgethdbyuserid);
                string apidatahdbyuserid = await responsegethdbyuserid.Content.ReadAsStringAsync();
                var lstHDByUserId = JsonConvert.DeserializeObject<List<HoaDon>>(apidatahdbyuserid);
                if (lstHDByUserId == null)
                {
                    return BadRequest("Không tìm thấy thông tin người dùng, lỗi 456");
                }
                var hd = lstHDByUserId.Where(x => x.TrangThai == 0).OrderByDescending(o => o.NgayTaoDon).FirstOrDefault();
                if(hd == null)
                {
                    return BadRequest("Chung tôi không tìm thấy thông tin của bạn, lỗi 461");
                }

                // đẩy dữ liệu từ Giỏ Hàng Chi Tiết qua Hóa Đơn Chi Tiết
                foreach (var item in GHCB)
                {
                    var newHDCT = new HoaDonChiTiet
                    {
                        MaHoaDonChiTiet = Guid.NewGuid().ToString(),
                        MaHoaDon = hd.MaHoaDon,
                        SKU = item.SKU,
                        TenSp = item.TenSp,
                        SoLuongMua = item.SoLuong,
                        DonGia = item.ThanhTien,
                        TrangThai = 1

                    };
                    var urltaoHDCT = $"https://localhost:7197/api/ChiTietHoaDon/TaoHoaDonCT";
                    var contenttaoHDCT = new StringContent(JsonConvert.SerializeObject(newHDCT), Encoding.UTF8, "application/json");
                    var responsetaoHDCT = await _httpClient.PostAsync(urltaoHDCT, contenttaoHDCT);
                    if (!responsetaoHDCT.IsSuccessStatusCode)
                    {
                        return BadRequest("Không thể tạo hóa đơn chi tiết, lỗi 497");
                    }
                }
                var urlupdateHD = $"https://localhost:7197/api/HoaDon?maHD={hd.MaHoaDon}&MaVoucher={null}&tenkh={maND}&sdt={nd.PhoneNumber}&phiship={tienship}&tongtien={tongtiendonhang}&pttt={4}&phanloai={2}&ghichu={"Không"}&tinh={tinh}&huyen={huyen}&xa={xa}&cuthe={cuthe}";
                var contentupdateHD = new StringContent("Update thành công");
                var responseUpdateHD = await _httpClient.PutAsync(urlupdateHD, contentupdateHD);
                if(responseUpdateHD.IsSuccessStatusCode)
                {
                    foreach(var item in GHCB)
                    {
                        _appDbContext.GioHangChiTiet.Remove(item);
                        _appDbContext.SaveChanges();
                    }
                    
                    // update thông tin giỏ hàng
                    var urlupdateGH = $"https://localhost:7197/api/GioHang/UpdateGH/{Ghcb.MaGioHang}?tongtien={0}&trangthai=1";
                    var contentupdateGH = new StringContent("Cập nhật thành công");

                    var responseUpdateGH = await _httpClient.PutAsync(urlupdateGH, contentupdateGH);
                    if (responseUpdateGH.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GioHangCuaBan", "CustomerHome", new { areas = "Customer" });
                    }
                }
                return BadRequest("Đặt hàng không thành công,634");
            }
            return BadRequest("Tạo hóa đơn sẵn không thành công, lỗi 636");

        }
    }
}
