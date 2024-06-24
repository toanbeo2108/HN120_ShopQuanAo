using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.View.Areas.Admin.Data;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using HN120_ShopQuanAo.Data.ViewModels;
using Newtonsoft.Json.Linq;


namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly HttpClient _httpClient;
       // private readonly UserManager<User> _userManager;
        string _mess;
        bool _stt;
        object _data = null;
        public HoaDonController()
        {
            _httpClient = new HttpClient();
        }

        // bán Hàng tại quầy 

        public async Task<IActionResult> Index()
        {

            var apiThanhToan = "https://localhost:7197/api/ThanhToan/GetAllThanhToan";
            var responThanhToan = await _httpClient.GetAsync(apiThanhToan);
            string apidaThanhToan = await responThanhToan.Content.ReadAsStringAsync();
            var lstThanhToan = JsonConvert.DeserializeObject<List<ThanhToan>>(apidaThanhToan);

            var apiThanhToan_hd = "https://localhost:7197/api/ThanhToan/GetAllThanhToan";
            var responThanhToan_hd = await _httpClient.GetAsync(apiThanhToan_hd);
            string apidaThanhToan_hd = await responThanhToan_hd.Content.ReadAsStringAsync();
            var lstThanhToan_hd = JsonConvert.DeserializeObject<List<ThanhToan_HoaDon>>(apidaThanhToan_hd);

            var apiurl_hd = "https://localhost:7197/api/HoaDon/GetAllHoaDon";
            var respon_hd = await _httpClient.GetAsync(apiurl_hd);
            string apiData_hd = await respon_hd.Content.ReadAsStringAsync();
            var lst_hd = JsonConvert.DeserializeObject<List<HoaDon>>(apiData_hd);
            var thanhtoanhoadon = from tt in lstThanhToan
                                  join lstt in lstThanhToan_hd
                                  on tt.MaPhuongThuc equals lstt.MaPhuongThuc
                                  select new ThanhToan_ThanhToanHistory
                                  {
                                      MaPhuongThuc = tt.MaPhuongThuc,
                                      TenPhuongThuc = tt.TenPhuongThuc,
                                      MoTa = tt.MoTa,
                                      NgayTao = tt.NgayTao,
                                      NgayThayDoi = tt.NgayThayDoi,
                                      // thanh toán historry
                                      MaPhuongThuc_HoaDon = lstt.MaPhuongThuc_HoaDon,
                                      NgayTaoThanhToan = lstt.NgayTao,
                                      NgayThayDoiThanhToan = lstt.NgayThayDoi,
                                 
                                  };

            ViewBag.thanhtoanhoadon = thanhtoanhoadon.ToList();
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
                                 TenSize = sz.TenSize
            };

            //if (!string.IsNullOrEmpty(selectedColor) && selectedColor != "Chọn màu")
            //{
            //    joinedData = joinedData.Where(x => x.TenMau == selectedColor);
            //}

            //if (!string.IsNullOrEmpty(selectedSize) && selectedSize != "Chọn size")
            //{
            //    joinedData = joinedData.Where(x => x.TenSize == selectedSize);
            //}

            // Áp dụng tìm kiếm theo Tên sản phẩm (TenSP)
            //if (!string.IsNullOrEmpty(searchText))
            //{
            //    joinedData = joinedData.Where(x => x.TenSP.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
            //}


            var litspctview = joinedData.ToList();
            ViewBag.JoinedData = litspctview;
     

            if (litspctview == null)
            {
                return BadRequest();
            }

            return View(litspctview);
        }

        [HttpPost, Route("Add-hoadon")]
        public async Task<IActionResult> Add_HoaDon(HoaDon hd)
        {
          
            var apiurlcr = "https://localhost:7197/api/HoaDon/CreateHoaDon";    
            var content = new StringContent(JsonConvert.SerializeObject(hd), Encoding.UTF8, "application/json");
            var responcr = await _httpClient.PostAsync(apiurlcr, content);
            string apiData = await responcr.Content.ReadAsStringAsync();

            if (responcr.StatusCode == System.Net.HttpStatusCode.Created)
            {

                _stt = true;
                _mess = "thêm thành công!";

            }
            else
            {
                _stt = false;
                _mess = "Thêm thất bại";
            }
            return Json(new
            {
                status = _stt,
                message = _mess
            });
        }

        // Quản lý hóa đơn
        [HttpGet]
        public async Task<IActionResult> GetAllHoaDon()
        {
            var apiurltt = $"https://localhost:7197/api/ThanhToan/GetAllThanhToan";
            var respontt = await _httpClient.GetAsync(apiurltt);
            string apiDatatt = await respontt.Content.ReadAsStringAsync();
            var lstt = JsonConvert.DeserializeObject<List<ThanhToan>>(apiDatatt);
            ViewBag.lstt = lstt;

            var apiurl = "https://localhost:7197/api/HoaDon/GetAllHoaDon";
            var httpClient = new HttpClient();
            var respon = await _httpClient.GetAsync(apiurl);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<HoaDon>>(apiData);


            if (respon.IsSuccessStatusCode)
            {
                return View(lst);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet, Route("Detail-hoadon/{ma}")]
        public async Task<IActionResult> GetHoaDonByMa(string ma)
        {

            var apiurl = $"https://localhost:7197/api/HoaDon/GetAllHoaDonMa/{ma}";            
            var respon = await _httpClient.GetAsync(apiurl);
            string apiData = await respon.Content.ReadAsStringAsync();
            HoaDon detail = null;
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                detail = JsonConvert.DeserializeObject<HoaDon>(apiData);
                if (detail == null)
                {
                    _stt = false;
                    _mess = "Không tìm thấy";
                }
                else
                {
                    _stt = true;
                    _mess = "";
                    _data = detail;
                }


            }
            else
            {
                _stt = false;
                _mess = respon.ReasonPhrase + "";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
                data = _data
            });
        }


        [HttpGet]
        public async Task<IActionResult> DetailHoaDon(string ma)
        {
            var apiurlHDCT = "https://localhost:7197/api/ChiTietHoaDon/GetAll";
            var responHDCT = await _httpClient.GetAsync(apiurlHDCT);
            string apiDataHDCT = await responHDCT.Content.ReadAsStringAsync();
            var lstHDCT = JsonConvert.DeserializeObject<List<HoaDonChiTiet>>(apiDataHDCT);
            //var totalHoaDonChiTiet = lstHDCT.Count();
            //ViewBag.lstHDCT = "HDCT" + (totalHoaDonChiTiet + 1);
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
                                 TenSize = sz.TenSize
                             };

            var litspctview = joinedData.ToList();
            ViewBag.JoinedData = litspctview;
            var url = $"https://localhost:7197/api/HoaDon/GetHoaDonWithDetails/{ma}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var apiData = await response.Content.ReadAsStringAsync();
                var hoaDonWithDetailsList = JsonConvert.DeserializeObject<List<HoaDonWithDetailsViewModel>>(apiData);

                // Tìm hoặc lọc danh sách hoaDonWithDetailsList theo mã hoá đơn cần hiển thị chi tiết
                var hoaDonWithDetails = hoaDonWithDetailsList.FirstOrDefault(); // hoặc lọc theo mã hoá đơn cụ thể

                if (hoaDonWithDetails == null)
                {
                    return BadRequest();
                }
                else
                {
                    return View(hoaDonWithDetails);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost, Route("Update-hoadon")]
        public async Task<IActionResult> UpdateVoucher(HoaDon hd)
        {
            var url = $"https://localhost:7197/api/HoaDon/UpdateHoaDon";
            var content = new StringContent(JsonConvert.SerializeObject(hd), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(url, content);
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {

                _stt = true;
                _mess = "Cập nhật thành công!";

            }
            else
            {
                _stt = false;
                _mess = "Cập nhật thât bại!";
            }
            return Json(new
            {
                status = _stt,
                message = _mess
            });
        }

        [HttpGet, Route("Xoa/{ma}")]
        public async Task<IActionResult> DeleteVoucher(string ma, HoaDon hd)
        {
            var url = $"https://localhost:7197/api/HoaDon/DeleteHoaDon/{ma}";
            var content = new StringContent(JsonConvert.SerializeObject(hd), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(url, content);
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {

                _stt = true;
                _mess = "Xóa thanh cong!";

            }
            else
            {
                _stt = false;
                _mess = "Xóa that bai!";
            }
            return Json(new
            {
                status = _stt,
                message = _mess
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllHoaDonBySTT(int stt)
        {
            var url = $"https://localhost:7197/api/HoaDon/GetAllHoaDonBySTT/{stt}";
            var response = await _httpClient.GetAsync(url);

               
            
                var apiData = await response.Content.ReadAsStringAsync();
                var hoaDon = JsonConvert.DeserializeObject<List<HoaDon>>(apiData);

                var listhoaDon = hoaDon.Where(c => c.TrangThai == stt).ToList();
            return View(listhoaDon); 
        }
    }
}
