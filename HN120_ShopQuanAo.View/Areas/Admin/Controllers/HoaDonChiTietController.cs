using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class HoaDonChiTietController : Controller
    {
        private readonly HttpClient _httpClient;

        string _mess;
        bool _stt;
        object _data = null;
        public HoaDonChiTietController()
        {
            _httpClient = new HttpClient();
        }
        public async Task<IActionResult> Index()
        {
            var apiurlhd = "https://localhost:7197/api/HoaDon/GetAllHoaDon";
            var responhd = await _httpClient.GetAsync(apiurlhd);
            string apiDatahd = await responhd.Content.ReadAsStringAsync();
            var lsthd = JsonConvert.DeserializeObject<List<HoaDon>>(apiDatahd);
            ViewBag.lsthd = lsthd;
            var apiSKU = "https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
            var responSKU = await _httpClient.GetAsync(apiSKU);
            string apiDataSKU = await responSKU.Content.ReadAsStringAsync();
            var lstsku = JsonConvert.DeserializeObject<List<ChiTietSp>>(apiDataSKU);
            ViewBag.lstsku = lstsku;

            var apiurl = "https://localhost:7197/api/ChiTietHoaDon/GetAll";
            var httpClient = new HttpClient();
            var respon = await _httpClient.GetAsync(apiurl);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<HoaDonChiTiet>>(apiData);

            if (lst == null)
            {
                return BadRequest();
            }
            return View(lst);
        }
        [HttpPost, Route("Add-hoadonct")]
        public async Task<IActionResult> Add_HoaDonChiTiet([FromBody] List<HoaDonChiTiet> hdct)
        {
            try
            {
               
                var apiCTSP = "https://localhost:7197/api/CTSanPham/GetAllCTSanPham";
                var responCTSP = await _httpClient.GetAsync(apiCTSP);
                string apidataCTSP = await responCTSP.Content.ReadAsStringAsync();
                var lstCTSP = JsonConvert.DeserializeObject<List<ChiTietSp>>(apidataCTSP);


                var apiurl = "https://localhost:7197/api/ChiTietHoaDon/GetAll";
                var respon = await _httpClient.GetAsync(apiurl);
                string apiData = await respon.Content.ReadAsStringAsync();
                DateTime now = DateTime.Now;
                string formattedTime = now.ToString("yyyyMMddHHmmss");
                var lst = JsonConvert.DeserializeObject<List<HoaDonChiTiet>>(apiData);
                var totalHoaDonCT = lst.Count();
                foreach (var hdctList in hdct)
                {
                    hdctList.MaHoaDonChiTiet = ("HDCT" + formattedTime + (totalHoaDonCT + 1)).ToString(); // Gán mã hóa đơn chi tiết
                    totalHoaDonCT++; // Tăng tổng số hóa đơn chi tiết để cho lần tiếp theo
                    var ctsanpham = lstCTSP.FirstOrDefault(c => c.SKU == hdctList.SKU);
                   // if (ctsanpham != null)
                    //{
                    //    if (ctsanpham.SoLuongTon < 0)
                    //    {
                    //        return Json(new
                    //        {
                    //            status = false,
                    //            message = "Sản phẩm còn lại không đủ"
                    //        });
                    //    }
                    //}
                }
                var apiurlcr = "https://localhost:7197/api/ChiTietHoaDon/CreateHDCT";
                var content = new StringContent(JsonConvert.SerializeObject(hdct), Encoding.UTF8, "application/json");
                var responcr = await _httpClient.PostAsync(apiurlcr, content);

                if (responcr.IsSuccessStatusCode)
                {
                    
                    return Json(new
                    {
                        status = true,
                        message = "Thanh toán thành công"
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message = "Thanh toán thất bại"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
            finally
            {
                _httpClient.Dispose(); // Giải phóng tài nguyên HttpClient sau khi sử dụng
            }
        }


        [HttpGet, Route("Detail-hoadonCT/{ma}")]
        public async Task<IActionResult> Detail(string ma)
        {

            var url = $"https://localhost:7197/api/ChiTietHoaDon/GetAll";
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<HoaDonChiTiet>>(apiData);
            var hoadonct = lst.FirstOrDefault(x => x.MaHoaDonChiTiet == ma);
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (hoadonct == null)
                {
                    _stt = false;
                    _mess = "Không tìn thấy";
                }
                else
                {
                    _stt = true;
                    _mess = "";
                    _data = hoadonct;
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

        [HttpPost, Route("Update-hoadonct")]
        public async Task<IActionResult> UpdateHoaDonChiTiet([FromBody] List<HoaDonChiTiet> hdctList)
        {
            if (hdctList == null || !hdctList.Any())
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var url = $"https://localhost:7197/api/ChiTietHoaDon/Update";
            var content = new StringContent(JsonConvert.SerializeObject(hdctList), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                _stt = true;
                _mess = "Cập nhật thành công!";
            }
            else
            {
                _stt = false;
                _mess = "Cập nhật thất bại!";
            }

            return Json(new
            {
                status = _stt,
                message = _mess
            });
        }


        [HttpPost]
        [Route("Dell-HDCT/{ma}")]
        public async Task<IActionResult> DeleteHDCT(string ma)
        {
            var urlcthd = $"https://localhost:7197/api/ChiTietHoaDon/GetAll";
            var responcthd = await _httpClient.GetAsync(urlcthd);
            string apiDatacthd = await responcthd.Content.ReadAsStringAsync();
            var lstcthd = JsonConvert.DeserializeObject<List<HoaDonChiTiet>>(apiDatacthd);
            var hoadonct = lstcthd.FirstOrDefault(x => x.MaHoaDonChiTiet == ma);

            var url = $"https://localhost:7197/api/ChiTietHoaDon/Delete/{ma}";
            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                _stt = true;
                _data = hoadonct;
                _mess = "Xóa thành công!";
            }
            else
            {
                _stt = false;
                _mess = "Xóa thất bại!";
            }

            return Json(new
            {
                status = _stt,
                message = _mess,
                data = _data
            });
        }


    }
}
