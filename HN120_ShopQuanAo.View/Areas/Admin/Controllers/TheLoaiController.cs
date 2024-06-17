﻿using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class TheLoaiController : Controller
    {
        private HttpClient _httpClient;
        public TheLoaiController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7197/api/TheLoai/GetAllTheLoai
        //https://localhost:7197/api/TheLoai/add-TL?Tentl=1&MoTa=1&TrangThai=1
        //https://localhost:7197/api/TheLoai/update-TL
        [HttpGet]
        public async Task<IActionResult> AllTheLoaiManager()
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/TheLoai/GetAllTheLoai";
            //var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataBook);
            return View(lstBook);
        }
        [HttpGet]
        public IActionResult CreateTheLoai()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTheLoai(TheLoai bk)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7197/api/TheLoai/AddTL?Tentl={bk.TenTheLoai}&MoTa={bk.MoTa}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllTheLoaiManager", "TheLoai", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> TheLoaiDetail(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/TheLoai/GetAllTheLoai";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaTheLoai == id);
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
        public async Task<IActionResult> UpdateTheLoai(string id)
        {
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7197/api/TheLoai/GetAllTheLoai";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<TheLoai>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.MaTheLoai == id);
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
        public async Task<IActionResult> UpdateTheLoai(string id, TheLoai vc)
        {
            var urlBook = $"https://localhost:7197/api/TheLoai/UpdateTL/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllTheLoaiManager", "TheLoai", new { area = "Admin" });

        }
    }
}