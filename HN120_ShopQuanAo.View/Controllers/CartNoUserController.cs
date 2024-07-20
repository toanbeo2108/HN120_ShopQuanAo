using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.View.Controllers
{
    public class CartNoUserController : Controller
    {
        private readonly HttpClient _httpClient;
        public CartNoUserController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
