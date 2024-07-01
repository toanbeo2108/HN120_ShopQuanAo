using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class ThanhToanController : Controller
    {
        private HttpClient _httpClient;
        string _mess;
        bool _stt;
        object _data = null;
        public ThanhToanController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllThanhToanManager()
        {
            var url = $"https://localhost:7197/api/ThanhToan/GetAllThanhToan";
            //var httpClient = new HttpClient();
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst= JsonConvert.DeserializeObject<List<ThanhToan>>(apiData);
            return View(lst);
        }
        [HttpPost, Route("Add-ThanhToan")]
        public async Task<IActionResult> Add_ThanhToanManager(ThanhToan tt)
        {
            try
            {

                var urlc = "https://localhost:7197/api/ThanhToan/GetAllThanhToan";
                //var httpClient = new HttpClient();
                var responc = await _httpClient.GetAsync(urlc);
                string apiDatac = await responc.Content.ReadAsStringAsync();
                var lstc = JsonConvert.DeserializeObject<List<ThanhToan>>(apiDatac);
                var coun = lstc.Count();
                tt.MaPhuongThuc = (coun + 1).ToString() ;

                var apiurl = "https://localhost:7197/api/ThanhToan/CreateThanhToan";
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
        [HttpGet,Route("thanhtoan-detail/{ma}")]
        public async Task<IActionResult> ThanhToanManagerDetail(string ma)
        {
           
            var url= $"https://localhost:7197/api/ThanhToan/GetAllThanhToan";
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<ThanhToan>>(apiData);
            var hoadonct = lst.FirstOrDefault(x => x.MaPhuongThuc == ma);
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
        [HttpPost, Route("Update-thanhtoan")]
        public async Task<IActionResult> UpdateVoucher(ThanhToan tt)
        {
          var url = $"https://localhost:7197/api/ThanhToan/UpdateHoaDon";
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
        
        [HttpGet, Route("Xoa/{id}")]
        public async Task<IActionResult> DeleteVoucher(string id,ThanhToan tt)
        {
            var url = $"https://localhost:7197/api/ThanhToan/DeleteThanhToan/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(tt), Encoding.UTF8, "application/json");
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
    }
}
