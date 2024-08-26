using HN120_ShopQuanAo.View.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HN120_ShopQuanAo.View.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Filter(FilterProductDto filterDto, int pageNumber = 1, string sortBy = "name_asc")
        {
            // Lấy giá Min và Max từ API
            var PriceResponse = await _httpClient.GetStringAsync("https://localhost:7197/api/FilterSanPham/GetMinMaxPrice");
            var minMaxPrice = JsonConvert.DeserializeObject<MinMaxPriceDto>(PriceResponse);
            ViewBag.minMaxPrice = minMaxPrice;

            // Lấy danh sách Thương hiệu
            var thuongHieuResponse = await _httpClient.GetStringAsync("https://localhost:7197/api/ThuongHieu/GetAllThuongHieu");
            var thuongHieus = JsonConvert.DeserializeObject<List<ThuongHieuDto>>(thuongHieuResponse);

            // Lấy danh sách Size
            var sizeResponse = await _httpClient.GetStringAsync("https://localhost:7197/api/Size/GetAllSize");
            var sizes = JsonConvert.DeserializeObject<List<SizeDto>>(sizeResponse);

            // Lấy danh sách Màu sắc
            var mauSacResponse = await _httpClient.GetStringAsync("https://localhost:7197/api/MauSac/GetAllMauSac");
            var mauSacs = JsonConvert.DeserializeObject<List<MauSacDto>>(mauSacResponse);

            // Lấy danh sách Chất liệu
            var chatLieuResponse = await _httpClient.GetStringAsync("https://localhost:7197/api/ChatLieu/GetAllChatLieu");
            var chatLieus = JsonConvert.DeserializeObject<List<ChatLieuDto>>(chatLieuResponse);

            // Lấy danh sách Thể loại
            var theLoaiResponse = await _httpClient.GetStringAsync("https://localhost:7197/api/TheLoai/GetAllTheLoai");
            var theLoais = JsonConvert.DeserializeObject<List<TheLoaiDto>>(theLoaiResponse);

            // Tạo query string từ filterDto
            var queryString = new StringBuilder();
            if (!string.IsNullOrEmpty(filterDto.MaThuongHieu))
            {
                queryString.Append($"&MaThuongHieu={filterDto.MaThuongHieu}");
            }
            if (!string.IsNullOrEmpty(filterDto.MaTheLoai))
            {
                queryString.Append($"&MaTheLoai={filterDto.MaTheLoai}");
            }
            if (!string.IsNullOrEmpty(filterDto.MaChatLieu))
            {
                queryString.Append($"&MaChatLieu={filterDto.MaChatLieu}");
            }
            if (filterDto.MinPrice.HasValue)
            {
                queryString.Append($"&MinPrice={filterDto.MinPrice.Value}");
            }
            if (filterDto.MaxPrice.HasValue)
            {
                queryString.Append($"&MaxPrice={filterDto.MaxPrice.Value}");
            }
            if (!string.IsNullOrEmpty(filterDto.MaSize))
            {
                queryString.Append($"&MaSize={filterDto.MaSize}");
            }
            if (!string.IsNullOrEmpty(filterDto.MaMau))
            {
                queryString.Append($"&MaMau={filterDto.MaMau}");
            }

            // Lấy sản phẩm đã lọc từ API
            var productsResponse = await _httpClient.GetStringAsync($"https://localhost:7197/api/FilterSanPham/paged?pageNumber={pageNumber}&pageSize=9&sortBy={sortBy}{queryString}");
            var pagedProducts = JsonConvert.DeserializeObject<PagedResultDto<ProductWithPriceRangeDto>>(productsResponse);

            // Truyền dữ liệu đến View
            var viewModel = new FilterViewModel
            {
                MinPrice = minMaxPrice.MinPrice,
                MaxPrice = minMaxPrice.MaxPrice,
                ThuongHieus = thuongHieus,
                Sizes = sizes,
                MauSacs = mauSacs,
                ChatLieus = chatLieus,
                TheLoais = theLoais,
                Products = pagedProducts.Items,
                TotalItems = pagedProducts.TotalItems,
                PageNumber = pageNumber,
                PageSize = 9,
                SortBy = sortBy
            };

            return View(viewModel);
        }
    }
}
