using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HN120_ShopQuanAo.View.Areas.Customer.Controllers
{
    public class HoaDonCustomer : Controller
    {
        private readonly HttpClient _httpClient;
        public HoaDonCustomer()
        {
            _httpClient = new HttpClient();
        }
        [HttpGet]
        //https://localhost:7197/api/HoaDon/GetAllHoaDon
        public async Task<IActionResult> HoaDonCuaToi()
        {
            var maND = Request.Cookies["UserId"];
            string urlhoadon = $"https://localhost:7197/api/HoaDon/GetAllHDByUserId/UserId?UserId={maND}";
            var responseListHDCB = await _httpClient.GetAsync(urlhoadon);
            string apidataListHDCB = await responseListHDCB.Content.ReadAsStringAsync();
            var lstHDCB =  JsonConvert.DeserializeObject<List<HoaDon>>(apidataListHDCB);
            if(lstHDCB == null)
            {
                return BadRequest("Dữ liệu của bạn không hoàn chỉnh hoặc đã bị xóa");
            }
            lstHDCB = lstHDCB.OrderByDescending(x => x.NgayTaoDon).ToList();
            ViewBag.lstHDCB = lstHDCB;
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> XemChiTietHoaDon(string maHD)
        {
            string urlhoadon = $"https://localhost:7197/api/HoaDon/GetAllHoaDonMa/{maHD}";
            var responseListHDCB = await _httpClient.GetAsync(urlhoadon);
            string apidataHDCB = await responseListHDCB.Content.ReadAsStringAsync();
            var HDND = JsonConvert.DeserializeObject<HoaDon>(apidataHDCB);
            if(HDND == null)
            {
                return BadRequest("Sai code rồi, có lấy được mã đâu ?");
            }

            var url = $"https://localhost:7197/api/ChiTietHoaDon/GetAll";
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<HoaDonChiTiet>>(apiData);
            if(lst == null)
            {
                return BadRequest("Đon Hàng này bị null");
            }
            var lstSPDH = lst.Where(x => x.MaHoaDon == HDND.MaHoaDon).ToList();
            if(lstSPDH == null)
            {
                return BadRequest("Hoá Đơn này bị lỗi rồi");
            }
            ViewBag.MaHD = HDND.MaHoaDon; 
            ViewBag.lstSPDH = lstSPDH;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> HuyDon(string maHD)
        {
            var urlhuydon = $"https://localhost:7197/api/HoaDon/HuyDon/{maHD}";
            var contentupdateHD = new StringContent("Cập nhật thành công");
            var responseHuyDon = await _httpClient.PutAsync(urlhuydon, contentupdateHD);
            if (responseHuyDon.IsSuccessStatusCode)
            {
                return RedirectToAction("HoaDonCuaToi", "HoaDonCustomer", new { areas = "Customer" });
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
