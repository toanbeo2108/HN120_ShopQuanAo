using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly HttpClient _httpClient;
        
        string _mess;
        bool _stt;
        object _data = null;
        public HoaDonController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            //var apiuser = "https://localhost:7197/api/HoaDon/GetAllHoaDon";
            //var responuser = await _httpClient.GetAsync(apiuser);
            //string apiDataus = await responuser.Content.ReadAsStringAsync();
            //var lstus = JsonConvert.DeserializeObject<List<User>>(apiDataus);
            //ViewBag.lstus = lstus;
            var apivc = "https://localhost:7197/GetAllVoucher";
            var responvc = await _httpClient.GetAsync(apivc);
            string apidatavc = await responvc.Content.ReadAsStringAsync();
            var lstvc = JsonConvert.DeserializeObject<List<Voucher>>(apidatavc);
            ViewBag.lstvc = lstvc;
            var apiurl = "https://localhost:7197/api/HoaDon/GetAllHoaDon";
            var httpClient = new HttpClient();
            var respon = await _httpClient.GetAsync(apiurl);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<HoaDon>>(apiData);

            if (lst == null)
            {
                return BadRequest();
            }
            return View(lst);
        }
        [HttpPost, Route("Add-hoadon")]
        public async Task<IActionResult> Add_HoaDon(HoaDon hd)
        {
            var apiurl = "https://localhost:7197/api/HoaDon/GetAllHoaDon";
            var httpClient = new HttpClient();
            var respon = await _httpClient.GetAsync(apiurl);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<HoaDon>>(apiData);
            var apiurlcr = "https://localhost:7197/api/HoaDon/CreateHoaDon";    
            var totalHoaDon = lst.Count();
            hd.MaHoaDon = "HD" + (totalHoaDon + 1);
            var content = new StringContent(JsonConvert.SerializeObject(hd), Encoding.UTF8, "application/json");
            var responcr = await httpClient.PostAsync(apiurlcr, content);

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
        [HttpGet, Route("Detail-hoadon/{ma}")]
        public async Task<IActionResult> Detail(string ma)
        {
           
            var url = $"https://localhost:7197/api/HoaDon/GetAllHoaDon";
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<HoaDon>>(apiData);
            var hoadon = lst.FirstOrDefault(x => x.MaHoaDon == ma);
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (hoadon == null)
                {
                    _stt = false;
                    _mess = "Không tìn thấy";
                }
                else
                {
                    _stt = true;
                    _mess = "";
                    _data = hoadon;
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
    }
}
