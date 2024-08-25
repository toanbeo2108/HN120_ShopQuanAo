using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HN120_ShopQuanAo.Data.Models;
using System.Collections.Generic;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class ChatLieuController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChatLieuController> _logger;

        public ChatLieuController(ILogger<ChatLieuController> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllChatLieuManager(string searchString)
        {
            var urlBook = $"https://localhost:7197/api/ChatLieu/GetAllChatLieu";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChatLieu>>(apiDataBook);

            if (!string.IsNullOrEmpty(searchString))
            {
                lstBook = lstBook.Where(x => x.TenChatLieu.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewData["CurrentFilter"] = searchString;
            return View(lstBook);
        }

        [HttpGet]
        public async Task<IActionResult> DSChatLieu()
        {
            var urlBook = $"https://localhost:7197/api/ChatLieu/GetAllChatLieu";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstCL = JsonConvert.DeserializeObject<List<ChatLieu>>(apiDataBook);
            return Json(lstCL);

        }

        [HttpPost]
        public async Task<IActionResult> CreateChatLieu(string TenChatLieu)
        {
            var bk = new ChatLieu { TenChatLieu = TenChatLieu};
            if (await IsDuplicateChatLieu(bk.TenChatLieu))
            {
                return Json(false);
            }
            var urlBook = $"https://localhost:7197/api/ChatLieu/AddChatLieu?TenChatLieu={bk.TenChatLieu}";
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PostAsync(urlBook, content);

            if (respon.IsSuccessStatusCode)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpGet]
        public async Task<IActionResult> ChatLieuDetail(string id)
        {
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
            if (await IsDuplicateChatLieu(vc.TenChatLieu, id))
            {
                TempData["errorMessage"] = "Tên chất liệu đã tồn tại.";
                return View(vc);
            }


            var urlBook = $"https://localhost:7197/api/ChatLieu/EditChatLieu/{id}";

            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            return RedirectToAction("AllChatLieuManager", "ChatLieu", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusChatLieuKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/ChatLieu/UpdateStatusChatLieu/{id}?_ctsp=1";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Kinh Doanh: {Error}", errorMessage);
                return BadRequest("Failed to update status to Kinh Doanh.");
            }
            return RedirectToAction("AllChatLieuManager");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusChatLieuKKD(string id)
        {
            var urlBook = $"https://localhost:7197/api/ChatLieu/UpdateStatusChatLieu/{id}?_ctsp=0";
            var response = await _httpClient.PutAsync(urlBook, null);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to update status to Không Kinh Doanh: {Error}", errorMessage);
                return BadRequest("Failed to update status to Không Kinh Doanh.");
            }
            return RedirectToAction("AllChatLieuManager");
        }

        private async Task<bool> IsDuplicateChatLieu(string tenChatLieu, string id = null)
        {
            var urlBook = $"https://localhost:7197/api/ChatLieu/GetAllChatLieu";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<ChatLieu>>(apiDataBook);

            if (id == null)
            {
                return lstBook.Any(x => x.TenChatLieu == tenChatLieu);
            }
            else
            {
                return lstBook.Any(x => x.TenChatLieu == tenChatLieu && x.MaChatLieu != id);
            }
        }
    }
}