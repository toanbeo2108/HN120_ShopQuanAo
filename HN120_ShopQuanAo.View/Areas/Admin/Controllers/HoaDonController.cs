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
using HN120_ShopQuanAo.Data.Configurations;
using System.Net;


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
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = HttpContext.User.Identity.Name; // Lấy tên người dùng

                var emailClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                var email = emailClaim != null ? emailClaim.Value : "Email not found";

                // Tạo một đối tượng view model để truyền thông tin đến view
                var model = new UserInfoViewModel
                {
                    UserId = userId,
                    UserName = userName
                };

                ViewBag.Model = model;
            }
            else
            {
                return RedirectToAction("Login", "Home", new { area = "" });

            }
            return View();
        }
        public async Task<IActionResult> Index()
        {

           
            Random r = new Random();
            ViewBag.RandomString = GenerateRandomString(r, 10);
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = HttpContext.User.Identity.Name; // Lấy tên người dùng
                var token = Request.Cookies["Token"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var urluser = $"https://localhost:7197/api/User/GetUsersByRole?roleName=admin";
                var responseuser = await _httpClient.GetAsync(urluser);
                string apiDataUser = await responseuser.Content.ReadAsStringAsync();
                var ListUseradmin = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
                var fullname = ListUseradmin.FirstOrDefault(c=>c.Id == userId);
                // Tạo một đối tượng view model để truyền thông tin đến view
                var model = new UserInfoViewModel
                {
                    UserId = userId,
                    UserName = fullname.FullName
                };

                ViewBag.Model = model;
            }
            
           
         //   ViewBag.ListUseradmin = ListUseradmin;


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
                       where ad.Status == 1
                       select new {
                       idUser = us.Id,
                       Ten = us.FullName,
                       tinhthanh = ad.City,
                       quanhuyen = ad.District,
                       xaphuong = ad.Ward,
                       cuthe = ad.Street,
                       sdt = ad.PhoneNumber
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
                                 TenSize = sz.TenSize,
                                 Dongia = ctsp.DonGia
            };
            var litspctview = joinedData.ToList();
            ViewBag.JoinedData = litspctview;
     

            if (litspctview == null)
            {
                return BadRequest();
            }

            return View(litspctview);
        }
        [HttpPost, Route("Update_soluongCTsanpham")]
        public async Task<IActionResult> Updatesoluongchitietsp([FromBody] List<ChiTietSp> ctsp)
        {
            //https://localhost:7197/api/CTSanPham/UpdateChiTietSp
            var url = $"https://localhost:7197/api/CTSanPham/UpdateChiTietSp";
            var content = new StringContent(JsonConvert.SerializeObject(ctsp), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PostAsync(url, content);
            if (respon.IsSuccessStatusCode)
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
        public async Task<IActionResult> QuanLyHoaDon( string sdt, int trangthai, string phanLoai, int pageNumber = 1, int pageSize = 6)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

            var getallhoadon = from hd in (
                               from h in lst
                               join us in Listaccout on h.UserID equals us.Id
                               select new
                               {
                                   h.MaHoaDon,
                                   h.UserID,
                                   us.FullName,
                                   h.MaVoucher,
                                   h.NgayTaoDon,
                                   h.NgayThayDoi,
                                   h.SoDienThoai,
                                   h.PhiShip,
                                   h.TongGiaTriHangHoa,
                                   h.PhuongThucThanhToan,
                                   h.PhanLoai,
                                   h.TrangThai
                               }
                               )
                               join ttkh in (
                               from h in lst
                               join us in Listaccout on h.TenKhachHang equals us.Id
                               select new
                               {
                                   h.MaHoaDon,
                                   h.TenKhachHang,
                                   us.FullName,
                                   us.PhoneNumber
                               }

                               ) on hd.MaHoaDon equals ttkh.MaHoaDon
                               select new HoaDonWithDetailsViewModel
                               {

                                   MaHoaDon_ = hd.MaHoaDon,
                                   tennhanhvien = hd.FullName,
                                   MaVC = hd.MaVoucher,
                                   NgayTao = hd.NgayTaoDon,
                                   Ngayupdate = hd.NgayThayDoi,
                                   tenkhachhang  = ttkh.FullName ,
                                   sdtkhhang = ttkh.PhoneNumber ,
                                   sdtnhahang = hd.SoDienThoai ,
                                   PhiShip_ = hd.PhiShip,
                                   Tonggiatri = hd.TongGiaTriHangHoa,
                                   tt = hd.TrangThai,
                                   phanloai = hd.PhanLoai,
                                   pttt = hd.PhuongThucThanhToan
                               };

            if (!string.IsNullOrEmpty(sdt))
            {
                getallhoadon = getallhoadon.Where(hd => hd.sdtnhahang.Contains(sdt) || hd.sdtkhhang.Contains(sdt));
            }
            if (trangthai != 0) // Assuming 0 means all statuses
            {
                getallhoadon = getallhoadon.Where(hd => hd.tt == trangthai);
            }
            if (!string.IsNullOrEmpty(phanLoai))
            {
                getallhoadon = getallhoadon.Where(hd => hd.phanloai == phanLoai);
            }
            var sortedList = getallhoadon.OrderByDescending(hoaDon => hoaDon.Ngayupdate);

            // Apply pagination
            var pagedResult = sortedList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            if (respon.IsSuccessStatusCode)
            {
                ViewBag.CurrentPage = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)sortedList.Count() / pageSize);
                ViewBag.TrangThai = trangthai;
                ViewBag.PhanLoai = phanLoai;
                ViewBag.SDT = sdt;
                return View(pagedResult);
            }
            else
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> GetHoaDonByMa(string ma)
        {
            #region
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urluser = $"https://localhost:7197/api/User/GetUsersByRole?roleName=admin";
            var responseuser = await _httpClient.GetAsync(urluser);
            string apiDataUser = await responseuser.Content.ReadAsStringAsync();
            var ListUseradmin = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            ViewBag.ListUseradmin = ListUseradmin;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = HttpContext.User.Identity.Name; // Lấy tên người dùng
                var fullname = ListUseradmin.FirstOrDefault(c => c.Id == userId);
                // Tạo một đối tượng view model để truyền thông tin đến view
                var model = new UserInfoViewModel
                {
                    UserId = userId,
                    UserName = fullname.FullName
                };

                ViewBag.Model = model;
            }
            var apiurlhoadon = "https://localhost:7197/api/HoaDon/GetAllHoaDon";
        
            var responhoadon = await _httpClient.GetAsync(apiurlhoadon);
            string apiDatahoadon = await responhoadon.Content.ReadAsStringAsync();
            var lsthoadon = JsonConvert.DeserializeObject<List<HoaDon>>(apiDatahoadon);

            //var urlusers = $"https://localhost:7197/api/UserAddress/GetAll";
            //var responseusers = await _httpClient.GetAsync(urlusers);
            //string apiDataUsers = await responseusers.Content.ReadAsStringAsync();
            //var ListUsers = JsonConvert.DeserializeObject<List<DeliveryAddressModel>>(apiDataUsers);

            var urluseraccout = $"https://localhost:7197/api/User/GetAllAccount";
            var responaccout = await _httpClient.GetAsync(urluseraccout);
            string apiDataaccout = await responaccout.Content.ReadAsStringAsync();
            var Listaccout = JsonConvert.DeserializeObject<List<User>>(apiDataaccout);
            ViewBag.Listaccout = Listaccout;
            var getallhoadon = from hd in (
                             from h in lsthoadon
                             join us in Listaccout on h.UserID equals us.Id
                             select new
                             {
                                 h.MaHoaDon,
                                 h.UserID,
                                 h.SoDienThoai,
                                 //h.PhiShip,
                                 //h.TongGiaTriHangHoa,
                                 //h.PhuongThucThanhToan,
                                 //h.PhanLoai,
                                 //h.TrangThai,
                                 //h.TinhThanh,
                                 //h.QuanHuyen,
                                 //h.XaPhuong,
                                 //h.Cuthe,
                                 //h.Ghichu
                             }
                             )
                               join ttkh in (
                               from h in lsthoadon
                               join us in Listaccout on h.TenKhachHang equals us.Id
                               select new
                               {
                                   h.MaHoaDon,
                                   h.TenKhachHang,
                                   us.FullName,
                                   us.PhoneNumber
                               }

                               ) on hd.MaHoaDon equals ttkh.MaHoaDon 
                               where hd.MaHoaDon ==  ma
                               select new HoaDonWithDetailsViewModel
                               {

                                   MaHoaDon_ = hd.MaHoaDon,
                                   IDkh =ttkh.TenKhachHang,
                                   //tennhanhvien = hd.FullName,
                                   //MaVC = hd.MaVoucher,
                                   //NgayTao = hd.NgayTaoDon,
                                   //Ngayupdate = hd.NgayThayDoi,
                                   tenkhachhang = ttkh.FullName,
                                   sdtkhhang = ttkh.PhoneNumber,
                                   //sdtnhahang = hd.SoDienThoai,
                                  // PhiShip_ = hd.PhiShip,
                                  // Tonggiatri = hd.TongGiaTriHangHoa,
                                  // tt = hd.TrangThai,
                                  // phanloai = hd.PhanLoai,
                                  // pttt = hd.PhuongThucThanhToan,
                                  // tinhthanh_=hd.TinhThanh,
                                  // quanhuyen=hd.QuanHuyen,
                                  //xaphuong =hd.XaPhuong,
                                  //cuthe =hd.Cuthe,
                                  // ghichu=hd.Ghichu
                               };

            ViewBag.getallhoadon = getallhoadon;
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

            var apiurltt = $"https://localhost:7197/api/ThanhToan/GetAllThanhToan";
            var respontt = await _httpClient.GetAsync(apiurltt);
            string apiDatatt = await respontt.Content.ReadAsStringAsync();
            var lstt = JsonConvert.DeserializeObject<List<ThanhToan>>(apiDatatt);
            ViewBag.lstt = lstt;
            var apiurlhdct = "https://localhost:7197/api/ChiTietHoaDon/GetAll";

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
                                 TenSize = sz.TenSize,
                                 Dongia = ctsp.DonGia
                             };
            var litspctview = joinedData.ToList();
            ViewBag.JoinedData = litspctview;
            #endregion


            //=====================================
            var urllshd = "https://localhost:7197/api/LichSuHoaDon/GetAll";
            var responlshd = await _httpClient.GetAsync(urllshd);
            string apidalshd = await responlshd.Content.ReadAsStringAsync();
            var lichsuhoadon = JsonConvert.DeserializeObject<List<HoaDon_History>>(apidalshd);
            var listLSHD = lichsuhoadon.Where(c => c.MaHoaDon == ma);
            ViewBag.ListLSHD = listLSHD;
            //====================================
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
                var urluseraccout = $"https://localhost:7197/api/User/GetAllAccount";
                var responaccout = await _httpClient.GetAsync(urluseraccout);
                string apiDataaccout = await responaccout.Content.ReadAsStringAsync();
                var Listaccout = JsonConvert.DeserializeObject<List<User>>(apiDataaccout);
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
        [HttpGet,Route("GetDanhSachVouCher/{check}")]
        public async Task<IActionResult> GetAllVoucher(decimal check)
        {
            var apiVC = "https://localhost:7197/GetAllVoucher";
            var responVC = await _httpClient.GetAsync(apiVC);
            string apidaVC = await responVC.Content.ReadAsStringAsync();
            var lstVC = JsonConvert.DeserializeObject<List<Voucher>>(apidaVC);
            var listVckhadung = lstVC.Where(c => c.GiaGiamToiThieu <= check).ToList();

            if (listVckhadung.Any())
            {
                return Ok(listVckhadung);
            }
            else
            {
                return NoContent();
            }
        }
        [HttpGet, Route("GetDanhSachUser")]
        public async Task<IActionResult> GetAlluser()
        {
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
                       where ad.Status == 1
                       select new
                       {
                           idUser = us.Id,
                           Ten = us.FullName,
                           tinhthanh = ad.City,
                           quanhuyen = ad.District,
                           xaphuong = ad.Ward,
                           cuthe = ad.Street,
                           sdt = us.PhoneNumber,
                           sdtnhanhhang = ad.PhoneNumber,
                           ngnhanhang = ad.Consignee
                       };

            if (user.ToList().Any())
            {
                return Ok(user.ToList());
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost, Route("Add_AdressFlash")]

        public async Task<IActionResult> AddUserAddress(string sdt, string sdtnhanhang, DeliveryAddress delivery)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // lấy danh sach user => tìm ra id khách vừa thêm vừa thêm
            var getalluserByroleUrl = "https://localhost:7197/api/User/GetUsersByRole?roleName=User";
            var responuserByrole = await _httpClient.GetAsync(getalluserByroleUrl);
            string apidatauserByrole = await responuserByrole.Content.ReadAsStringAsync();
            var userByrole = JsonConvert.DeserializeObject<List<User>>(apidatauserByrole);
            var name = userByrole.FirstOrDefault(c=>c.PhoneNumber ==  sdt);
            string id = name.Id;
            //  sau khi lấy được Id thêm địa chỉ người dùng
            delivery.UserID = id;
            var urlthemdiachi = $"https://localhost:7197/api/UserAddress/Create";
            var content = new StringContent(JsonConvert.SerializeObject(delivery), Encoding.UTF8, "application/json");
            var responsethemdiachi = await _httpClient.PostAsync(urlthemdiachi, content);

            //  lấy danh sach địa chỉ khách vừa thêm => tìm DeliveryAddressID = sdt nhận hàng
            var addressUrl = $"https://localhost:7197/api/UserAddress/GetByUserID?id={id}";
            var addressResponse = await _httpClient.GetAsync(addressUrl);
            string addressData = await addressResponse.Content.ReadAsStringAsync();
            var userAddresses = JsonConvert.DeserializeObject<List<DeliveryAddress>>(addressData);
            var sdtdiachivuathem = userAddresses.FirstOrDefault(c => c.PhoneNumber == sdtnhanhang);
            var idnhanhang = sdtdiachivuathem.DeliveryAddressID;
            // set defaut địa chỉ 
            var urlsetdefau = $"https://localhost:7197/api/UserAddress/SetDefaultAddress/?id=" + idnhanhang;
            var response = await _httpClient.PostAsync(urlsetdefau, null);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Lỗi cài đặt địa chỉ mặc định" });
            }
        }


    }
}
