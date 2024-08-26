using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.View.Areas.Admin.Data;
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

			var apiSp = "https://localhost:7197/api/SanPham/GetAllSanPham";
			var responsp = await _httpClient.GetAsync(apiSp);
			string apiDataSP = await responsp.Content.ReadAsStringAsync();
			var lstsp = JsonConvert.DeserializeObject<List<SanPham>>(apiDataSP);

			var apiCTSP = "https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
			var responCTSP = await _httpClient.GetAsync(apiCTSP);
			string apidataCTSP = await responCTSP.Content.ReadAsStringAsync();
			var lstCTSP = JsonConvert.DeserializeObject<List<ChiTietSp>>(apidataCTSP);

			var apiMauSac = "https://localhost:7197/api/MauSac/GetAllMauSac";
			var responMauSac = await _httpClient.GetAsync(apiMauSac);
			string apidaMauSac = await responMauSac.Content.ReadAsStringAsync();
			var lstMauSac = JsonConvert.DeserializeObject<List<MauSac>>(apidaMauSac);

			var apiSize = "https://localhost:7197/api/Size/GetAllSize";
			var responSize = await _httpClient.GetAsync(apiSize);
			string apidaSize = await responSize.Content.ReadAsStringAsync();
			var lstSize = JsonConvert.DeserializeObject<List<Size>>(apidaSize);

			var apiVC = "https://localhost:7197/GetAllVoucher";
			var responVC = await _httpClient.GetAsync(apiVC);
			string apidaVC = await responVC.Content.ReadAsStringAsync();
			var lstVC = JsonConvert.DeserializeObject<List<Voucher>>(apidaVC);
			ViewBag.lstVC = lstVC;
			var joinedData = from sp in lstsp
							 join ctsp in lstCTSP on sp.MaSp equals ctsp.MaSp
                             join cthd in lstSPDH on ctsp.SKU equals cthd.SKU
							 join ms in lstMauSac on ctsp.MaMau equals ms.MaMau
							 join sz in lstSize on ctsp.MaSize equals sz.MaSize
							 select new ChiTietSPView
							 {
								 MaSp = sp.MaSp,
								 MaThuongHieu = sp.MaThuongHieu,
								 MaTheLoai = sp.MaTheLoai,
								 UrlAvatar = sp.UrlAvatar,
								 TenSP = sp.TenSP,
								 Mota = sp.Mota,
								 TongSoLuong = sp.TongSoLuong,
								 SKU = ctsp.SKU,
								 MaSize = ctsp.MaSize,
								 MaMau = ctsp.MaMau,
								 MaKhuyenMai = ctsp.MaKhuyenMai,
								 // MaChatLieu = ctsp.MaChatLieu,
								 UrlAnhSpct = ctsp.UrlAnhSpct,
								 GiaBan = ctsp.GiaBan,
								 SoLuongTon = ctsp.SoLuongTon,
								 TrangThai = ctsp.TrangThai,
								 TenMau = ms.TenMau,
								 TenSize = sz.TenSize,
								 Dongia = ctsp.DonGia
							 };
			var litspctview = joinedData.ToList();
			ViewBag.JoinedData = litspctview;
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
