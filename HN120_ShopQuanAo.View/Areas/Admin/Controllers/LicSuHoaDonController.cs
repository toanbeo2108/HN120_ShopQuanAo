using HN120_ShopQuanAo.Data.Configurations;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class LicSuHoaDonController : Controller
    {
        private readonly HttpClient _httpClient;
        string _mess;
        bool _stt;
        object _data;
        public LicSuHoaDonController(HttpClient httpclient)
        {
                _httpClient = httpclient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet, Route("GetAll-LichSuHoaDon")]
        public async Task<IActionResult> GetAllLichSuHoaDon()
        {
            var urllshd = $"https://localhost:7197/api/LichSuHoaDon/GetAll";
            var responlshd = await _httpClient.GetAsync(urllshd);
          
            if (responlshd.IsSuccessStatusCode)
            {
                string apiDatalshd = await responlshd.Content.ReadAsStringAsync();
                var lstlshd = JsonConvert.DeserializeObject<List<HoaDon_History>>(apiDatalshd);
                _stt = true;
                _data = lstlshd;
                _mess = "OK";
            }
            else
            {
                _stt = false;
                _mess = "không tìm thấy";

            }
            return Json(new
            {
                status = _stt,
                message = _mess,
                data = _data
            });

        }
        [HttpGet, Route("GetAll-LichSuHoaDonBymahd")]
        public async Task<IActionResult> GetAllLichSuHoaDonbymahd(string ma)
        {
            var urllshd = $"https://localhost:7197/api/LichSuHoaDon/GetAllLichsuHoaDon";
            var responlshd = await _httpClient.GetAsync(urllshd);
            string apiDatalshd = await responlshd.Content.ReadAsStringAsync();
            var lstlshd = JsonConvert.DeserializeObject<List<HoaDon_History>>(apiDatalshd);
            var lshdbyma = lstlshd.FirstOrDefault(c => c.MaHoaDon == ma); 
            if (responlshd.IsSuccessStatusCode)
            {
                _stt = true;
                _data = lshdbyma;
                _mess = "OK";
            }
            else
            {
                _stt = false;
                _mess = "không tìm thấy";

            }
            return Json(new
            {
                status = _stt,
                message = _mess,
                data = _data
            });

        }
        [HttpPost, Route("Add-lichsuhoadon")]
        public async Task<IActionResult> Add_lichsuhoadonn(HoaDon_History lshd)
        {
            DateTime now = DateTime.Now;
            string formattedTime = now.ToString("yyyyMMddHHmmss");

            lshd.LichSuHoaDonID = ("HDCT" + formattedTime).ToString();
            var apiurlcrlshd = $"https://localhost:7197/api/LichSuHoaDon/CreateHDCT";          
            var contentlshd = new StringContent(JsonConvert.SerializeObject(lshd), Encoding.UTF8, "application/json");
            var responcrlshd = await _httpClient.PostAsync(apiurlcrlshd, contentlshd);
            string apiDatalshd = await responcrlshd.Content.ReadAsStringAsync();
            if (responcrlshd.IsSuccessStatusCode)
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
    }
}
