using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.View.Areas.Employee.Controllers
{
    public class EmployeeHomeController : Controller
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
