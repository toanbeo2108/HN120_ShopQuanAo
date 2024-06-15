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
        public async Task<IActionResult> Add_HoaDon(HoaDonChiTiet hd)
        {
            var apiurl = "https://localhost:7197/api/ChiTietHoaDon/GetAll";
            var httpClient = new HttpClient();
            var respon = await _httpClient.GetAsync(apiurl);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<HoaDonChiTiet>>(apiData);
            var apiurlcr = "https://localhost:7197/api/ChiTietHoaDon/CreateHDCT";
            var totalHoaDonCT = lst.Count();
            hd.MaHoaDonChiTiet = "HDCT" + (totalHoaDonCT + 1);
            var content = new StringContent(JsonConvert.SerializeObject(hd), Encoding.UTF8, "application/json");
            var responcr = await httpClient.PostAsync(apiurlcr, content);

            if (responcr.StatusCode == System.Net.HttpStatusCode.OK)
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
        public async Task<IActionResult> UpdateVoucher(HoaDonChiTiet hd)
        {
            var url = $"https://localhost:7197/api/ChiTietHoaDon/Update";
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

        [HttpGet, Route("DELETEHDCT/{ma}")]
        public async Task<IActionResult> DeleteVoucher(string ma, HoaDonChiTiet hd)
        {
            var url = $"https://localhost:7197/api/ChiTietHoaDon/Delete/{ma}";
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
