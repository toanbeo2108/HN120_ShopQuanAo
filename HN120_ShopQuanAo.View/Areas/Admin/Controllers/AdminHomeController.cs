﻿using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.View.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
