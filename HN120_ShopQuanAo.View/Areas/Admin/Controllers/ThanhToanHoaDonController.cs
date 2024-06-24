using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class ThanhToanHoaDonController : Controller
    {
        private HttpClient _httpClient;
        string _mess;
        bool _stt;
        object _data = null;
        public ThanhToanHoaDonController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllThanhToanHoaDonManager()
        {

            var apiHD = "https://localhost:7197/api/HoaDon/GetAllHoaDon";
            var responHD = await _httpClient.GetAsync(apiHD);
            string apiDataHD = await responHD.Content.ReadAsStringAsync();
            var lstHD = JsonConvert.DeserializeObject<List<HoaDon>>(apiDataHD);
            ViewBag.lstHD = lstHD;

            var urlp = $"https://localhost:7197/api/ThanhToan/GetAllThanhToan";
            //var httpClient = new HttpClient();
            var responp = await _httpClient.GetAsync(urlp);
            string apiDatap = await responp.Content.ReadAsStringAsync();
            var lstp = JsonConvert.DeserializeObject<List<ThanhToan>>(apiDatap);
            ViewBag.lstp = lstp;

            var url = $"https://localhost:7197/api/ThanhToanHoaDon/GetAllThanhToan_HoaDon";
            //var httpClient = new HttpClient();
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<ThanhToan_HoaDon>>(apiData);
            return View(lst);
        }
        [HttpPost, Route("Add-ThanhToanhoadon")]
        public async Task<IActionResult> Add_ThanhToanHoaDonManager(ThanhToan_HoaDon tt)
        {
            try
            {               
                var urlc = "https://localhost:7197/api/ThanhToanHoaDon/GetAllThanhToan_HoaDon";
                //var httpClient = new HttpClient();
                var responc = await _httpClient.GetAsync(urlc);
                string apiDatac = await responc.Content.ReadAsStringAsync();
                var lstc = JsonConvert.DeserializeObject<List<ThanhToan_HoaDon>>(apiDatac);
                var coun = lstc.Count();
                tt.MaPhuongThuc_HoaDon = "TTHD" + (coun + 1);

                var apiurl = $"https://localhost:7197/api/ThanhToanHoaDon/AddThanhToanThanhToan_HoaDon";
                var content = new StringContent(JsonConvert.SerializeObject(tt), Encoding.UTF8, "application/json");
                var respon = await _httpClient.PostAsync(apiurl, content);

                if (respon.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    _stt = true;
                    _mess = "Thêm thành công !";

                }
                else
                {
                    _stt = false;
                    _mess = "Thêm thất bại";
                }
                return Json(new
                {
                    status = _stt,
                    message = _mess,
                });
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
        [HttpGet, Route("thanhtoanhoadon-detail/{ma}")]
        public async Task<IActionResult> ThanhToanManagerDetail(string ma)
        {

            var url = $"https://localhost:7197/api/ThanhToanHoaDon/GetAllThanhToan_HoaDon";
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<ThanhToan_HoaDon>>(apiData);
            var hoadonct = lst.FirstOrDefault(x => x.MaPhuongThuc_HoaDon == ma);
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
        [HttpPost, Route("Update-thanhtoanhoadon")]
        public async Task<IActionResult> UpdateVoucher(ThanhToan_HoaDon tt)
        {
            var url = $"https://localhost:7197/api/ThanhToanHoaDon/UpdateThanhToan_HoaDon";
            var content = new StringContent(JsonConvert.SerializeObject(tt), Encoding.UTF8, "application/json");
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

    }
}
