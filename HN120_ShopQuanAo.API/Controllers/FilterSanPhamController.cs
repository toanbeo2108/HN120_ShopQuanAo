using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterSanPhamController : ControllerBase
    {
        private readonly IFilterProductRepository _filterProductRepository;

        public FilterSanPhamController(IFilterProductRepository filterProductRepository)
        {
            _filterProductRepository = filterProductRepository;
        }

        [HttpGet("filter")]
        public IActionResult GetFilteredProducts([FromQuery] FilterProductDto filterDto)
        {
            try
            {
                var products = _filterProductRepository.GetFilteredProducts(filterDto);
                return Ok(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFilteredProducts: {ex.Message}");
                return StatusCode(500, "An error occurred while filtering products.");
            }
        }

        [HttpGet("paged")]
        public IActionResult GetPagedProducts([FromQuery] FilterProductDto filterDto, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = null)
        {
            try
            {
                var pagedResult = _filterProductRepository.GetPagedProducts(filterDto, pageNumber, pageSize, sortBy);
                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPagedProducts: {ex.Message}");
                return StatusCode(500, "An error occurred while getting paged products.");
            }
        }

        [HttpGet("GetMinMaxPrice")]
        public IActionResult GetMinMaxPrice()
        {
            try
            {
                var minMaxPrice = _filterProductRepository.GetMinMaxPrice();
                return Ok(minMaxPrice);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMinMaxPrice: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving price range.");
            }
        }
    }
}
