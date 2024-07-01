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
using System.Security.Policy;
using System;


namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly HttpClient _httpClient;
       // private readonly UserManager<User> _userManager;
       // private readonly UserManager<User> _userManager;
        string _mess;
        bool _stt;
        object _data = null;
        public HoaDonController()
        {
            _httpClient = new HttpClient();
    
        }

        // bán Hàng tại quầy 
        private string GenerateRandomString(Random random, int length)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] randomString = new char[length];

            for (int i = 0; i < length; i++)
            {
                randomString[i] = letters[random.Next(letters.Length)];
            }

            return new string(randomString);
        }
        public async Task<IActionResult> BanHangTaiQuayView()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {

           
            Random r = new Random();
            ViewBag.RandomString = GenerateRandomString(r, 10);
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urluser = $"https://localhost:7197/api/User/GetUsersByRole?roleName=admin";
            var responseuser = await _httpClient.GetAsync(urluser);
            string apiDataUser = await responseuser.Content.ReadAsStringAsync();
            var ListUseradmin = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            ViewBag.ListUseradmin = ListUseradmin;
            
            var urlusers = $"https://localhost:7197/api/UserAddress/GetAll";
            var responseusers = await _httpClient.GetAsync(urlusers);
            string apiDataUsers = await responseusers.Content.ReadAsStringAsync();
            var ListUsers = JsonConvert.DeserializeObject<List<DeliveryAddressModel>>(apiDataUsers);

            var urluseraccout = $"https://localhost:7197/api/User/GetAllAccount";
            var responaccout = await _httpClient.GetAsync(urluseraccout);
            string apiDataaccout = await responaccout.Content.ReadAsStringAsync();
            var Listaccout = JsonConvert.DeserializeObject<List<User>>(apiDataaccout);

            var user = from us in Listaccout
                       join ad in ListUsers on us.Id equals ad.UserID
                       where ad.Status > 0
                       select new {
                       idUser = us.Id,
                       Ten = us.FullName,
                       tinhthanh = ad.City,
                       quanhuyen = ad.District,
                       xaphuong = ad.Ward,
                       cuthe = ad.Street,

                       };

            ViewBag.user = user.ToList();


            var apiThanhToan = "https://localhost:7197/api/ThanhToan/GetAllThanhToan";
            var responThanhToan = await _httpClient.GetAsync(apiThanhToan);
            string apidaThanhToan = await responThanhToan.Content.ReadAsStringAsync();
            var lstThanhToan = JsonConvert.DeserializeObject<List<ThanhToan>>(apidaThanhToan);
            ViewBag.lstThanhToan = lstThanhToan;
            var apiThanhToan_hd = "https://localhost:7197/api/ThanhToanHoaDon/GetAllThanhToan_HoaDon";
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
                             where ctsp.SoLuongTon > 0
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
     

            if (litspctview == null)
            {
                return BadRequest();
            }

            return View(litspctview);
        }
        [HttpGet, Route("getkhachhang")]
        public async Task<IActionResult> GetallLstKhachHang()
        {
            var urlusers = $"https://localhost:7197/api/User/GetUsersByRole?roleName=user";
            var responseusers = await _httpClient.GetAsync(urlusers);

            if (responseusers.IsSuccessStatusCode)
            {
                var apiDataUsers = await responseusers.Content.ReadAsStringAsync();
                var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUsers);

                // Lấy danh sách các đối tượng có dạng { id: ..., fullName: ... }
                var selectList = ListUser.Select(u => new { id = u.Id, fullName = u.FullName }).ToList();

                return Json(new
                {
                    status = true,
                    message = "Danh sách khách hàng",
                    data = selectList
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "Không có danh sách khách hàng",
                    data = new List<object>() // Hoặc có thể trả về một danh sách rỗng
                });
            }
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
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urluser = $"https://localhost:7197/api/User/GetUsersByRole?roleName=admin";
            var responseuser = await _httpClient.GetAsync(urluser);
            string apiDataUser = await responseuser.Content.ReadAsStringAsync();
            var ListUseradmin = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            ViewBag.ListUseradmin = ListUseradmin;

            var apiVC = "https://localhost:7197/GetAllVoucher";
            var responVC = await _httpClient.GetAsync(apiVC);
            string apidaVC = await responVC.Content.ReadAsStringAsync();
            var lstVC = JsonConvert.DeserializeObject<List<Voucher>>(apidaVC);
            ViewBag.lstVC = lstVC;

            var urluseraccout = $"https://localhost:7197/api/User/GetAllAccount";
            var responaccout = await _httpClient.GetAsync(urluseraccout);
            string apiDataaccout = await responaccout.Content.ReadAsStringAsync();
            var Listaccout = JsonConvert.DeserializeObject<List<User>>(apiDataaccout);
            ViewBag.Listaccout = Listaccout;

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
        public async Task<IActionResult> GetHoaDonByMa(string ma)
        {
            var urlusers = $"https://localhost:7197/api/UserAddress/GetAll";
            var responseusers = await _httpClient.GetAsync(urlusers);
            string apiDataUsers = await responseusers.Content.ReadAsStringAsync();
            var ListUsers = JsonConvert.DeserializeObject<List<DeliveryAddressModel>>(apiDataUsers);

            var urluseraccout = $"https://localhost:7197/api/User/GetAllAccount";
            var responaccout = await _httpClient.GetAsync(urluseraccout);
            string apiDataaccout = await responaccout.Content.ReadAsStringAsync();
            var Listaccout = JsonConvert.DeserializeObject<List<User>>(apiDataaccout);
            ViewBag.Listaccout = Listaccout;
            var user = from us in Listaccout
                       join ad in ListUsers on us.Id equals ad.UserID
                       where ad.Status > 0
                       select new
                       {
                           idUser = us.Id,
                           Ten = us.FullName,
                           tinhthanh = ad.City,
                           quanhuyen = ad.District,
                           xaphuong = ad.Ward,
                           cuthe = ad.Street,

                       };

            ViewBag.user = user.ToList();

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urluser = $"https://localhost:7197/api/User/GetUsersByRole?roleName=admin";
            var responseuser = await _httpClient.GetAsync(urluser);
            string apiDataUser = await responseuser.Content.ReadAsStringAsync();
            var ListUseradmin = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            ViewBag.ListUseradmin = ListUseradmin;

            var apiThanhToan_hd = "https://localhost:7197/api/ThanhToanHoaDon/GetAllThanhToan_HoaDon";
            var responThanhToan_hd = await _httpClient.GetAsync(apiThanhToan_hd);
            string apidaThanhToan_hd = await responThanhToan_hd.Content.ReadAsStringAsync();
            var lstThanhToan_hd = JsonConvert.DeserializeObject<List<ThanhToan_HoaDon>>(apidaThanhToan_hd);
            ViewBag.lstThanhToan_hd = lstThanhToan_hd;
            var apiVC = "https://localhost:7197/GetAllVoucher";
            var responVC = await _httpClient.GetAsync(apiVC);
            string apidaVC = await responVC.Content.ReadAsStringAsync();
            var lstVC = JsonConvert.DeserializeObject<List<Voucher>>(apidaVC);
            ViewBag.lstVC = lstVC;

            //var urluseraccout = $"https://localhost:7197/api/User/GetAllAccount";
            //var responaccout = await _httpClient.GetAsync(urluseraccout);
            //string apiDataaccout = await responaccout.Content.ReadAsStringAsync();
            //var Listaccout = JsonConvert.DeserializeObject<List<User>>(apiDataaccout);
            //ViewBag.Listaccout = Listaccout;

            var apiurltt = $"https://localhost:7197/api/ThanhToan/GetAllThanhToan";
            var respontt = await _httpClient.GetAsync(apiurltt);
            string apiDatatt = await respontt.Content.ReadAsStringAsync();
            var lstt = JsonConvert.DeserializeObject<List<ThanhToan>>(apiDatatt);
            ViewBag.lstt = lstt;


            var apiurlhdct = "https://localhost:7197/api/ChiTietHoaDon/GetAll";
         //   var httpClient = new HttpClient();
            var responhdct = await _httpClient.GetAsync(apiurlhdct);
            string apiDatahdct = await responhdct.Content.ReadAsStringAsync();
            var lsthdct = JsonConvert.DeserializeObject<List<HoaDonChiTiet>>(apiDatahdct);

            ViewBag.lsthdct = lsthdct;
            //=====================

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

            var joinedData = from sp in lstsp
                             join ctsp in lstCTSP on sp.MaSp equals ctsp.MaSp
                             join ms in lstMauSac on ctsp.MaMau equals ms.MaMau
                             join sz in lstSize on ctsp.MaSize equals sz.MaSize
                             where ctsp.SoLuongTon > 0
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

            //=====================================
            var apiurl = $"https://localhost:7197/api/HoaDon/GetAllHoaDonMa/{ma}";            
            var respon = await _httpClient.GetAsync(apiurl);
            string apiData = await respon.Content.ReadAsStringAsync();
            HoaDon detail = null;
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                detail = JsonConvert.DeserializeObject<HoaDon>(apiData);
                if (detail == null)
                {
                    return BadRequest();
                }
                else
                {
                    return View(detail);
                }
            }
            else
            {
                return BadRequest();
            }
            
        }
        [HttpPost, Route("Update-hoadon")]
        public async Task<IActionResult> UpdateHoaDon(HoaDon hd)
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
        [HttpGet, Route("Detail-hoadon/{ma}")]
        public async Task<IActionResult> GetHoaDonByMahd(string ma)
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

        [HttpGet, Route("Xoa/{ma}")]
        public async Task<IActionResult> DeleteHoaDon(string ma, HoaDon hd)
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
