using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using HN120_ShopQuanAo.Data.Models;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class ChatLieuController : Controller
    {
        private HttpClient _httpClient;
        public ChatLieuController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7197/api/ChatLieu/GetAllChatLieu
        //https://localhost:7197/api/ChatLieu/add-TH?TenChatLieu=vai&MoTa=1&TrangThai=1
        //https://localhost:7197/api/ChatLieu/update-TH/TH1
        [HttpGet]
        public async Task<IActionResult> AllChatLieuManager()
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/ChatLieu/GetAllChatLieu";
            //var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChatLieu>>(apiDataBook);
            return View(lstBook);
        }
        [HttpGet]
        public IActionResult CreateChatLieu()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateChatLieu(ChatLieu bk)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7197/api/ChatLieu/AddChatLieu?TenChatLieu={bk.TenChatLieu}&MoTa={bk.MoTa}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllChatLieuManager", "ChatLieu", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ChatLieuDetail(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/ChatLieu/GetAllChatLieu";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChatLieu>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaChatLieu == id);
            if (Book == null)
            {
                return BadRequest();
            }
            else
            {
                return View(Book);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateChatLieu(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/ChatLieu/GetAllChatLieu";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChatLieu>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaChatLieu == id);
            if (Book == null)
            {
                return BadRequest();
            }
            else
            {
                return View(Book);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateChatLieu(string id, ChatLieu vc)
        {
            var urlBook = $"https://localhost:7197/api/ChatLieu/UpdateChatLieu/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllChatLieuManager", "ChatLieu", new { area = "Admin" });

        }
    }
}
