using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Model;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HN120_ShopQuanAo.API.Responsitories
{
    public class FilterProductRepository : IFilterProductRepository
    {
        private readonly AppDbContext _context;
        public FilterProductRepository(AppDbContext context)
        {
            _context = context;
        }

        // Lọc sản phẩm dựa trên DTO đầu vào
        public IQueryable<ProductWithPriceRangeDto> GetFilteredProducts(FilterProductDto filterDto)
        {
            try
            {
                // Truy vấn sản phẩm dựa trên các điều kiện lọc
                var sanPhamQuery = _context.SanPham.AsQueryable();

                if (!string.IsNullOrEmpty(filterDto.MaThuongHieu))
                    sanPhamQuery = sanPhamQuery.Where(sp => sp.MaThuongHieu == filterDto.MaThuongHieu);

                if (!string.IsNullOrEmpty(filterDto.MaTheLoai))
                    sanPhamQuery = sanPhamQuery.Where(sp => sp.MaTheLoai == filterDto.MaTheLoai);

                if (!string.IsNullOrEmpty(filterDto.MaChatLieu))
                    sanPhamQuery = sanPhamQuery.Where(sp => sp.MaChatLieu == filterDto.MaChatLieu);

                // Lọc chi tiết sản phẩm theo giá
                var chiTietSpQuery = _context.ChiTietSp.AsQueryable();

                if (filterDto.MinPrice.HasValue)
                    chiTietSpQuery = chiTietSpQuery.Where(ct => ct.GiaBan >= filterDto.MinPrice.Value);

                if (filterDto.MaxPrice.HasValue)
                    chiTietSpQuery = chiTietSpQuery.Where(ct => ct.GiaBan <= filterDto.MaxPrice.Value);

                if (!string.IsNullOrEmpty(filterDto.MaSize))
                    chiTietSpQuery = chiTietSpQuery.Where(ct => ct.MaSize == filterDto.MaSize);

                if (!string.IsNullOrEmpty(filterDto.MaMau))
                    chiTietSpQuery = chiTietSpQuery.Where(ct => ct.MaMau == filterDto.MaMau);

                // Lấy danh sách mã sản phẩm phù hợp với điều kiện
                var maSpList = chiTietSpQuery.Select(ct => ct.MaSp).Distinct().ToList();

                // Lọc sản phẩm theo danh sách mã sản phẩm
                sanPhamQuery = sanPhamQuery.Where(sp => maSpList.Contains(sp.MaSp));

                // Chuyển đổi kết quả sang ProductWithPriceRangeDto
                var result = sanPhamQuery.Select(sp => new ProductWithPriceRangeDto
                {
                    MaSp = sp.MaSp,
                    TenSP = sp.TenSP,
                    UrlAvatar = sp.UrlAvatar,
                    MinPrice = 0,
                    MaxPrice = 0
                }).ToList();

                // Tính toán MinPrice và MaxPrice cho từng sản phẩm
                foreach (var dto in result)
                {
                    var chiTietSpQueryForDto = _context.ChiTietSp
                        .Where(ct => ct.MaSp == dto.MaSp);

                    if (filterDto.MinPrice.HasValue)
                        chiTietSpQueryForDto = chiTietSpQueryForDto.Where(ct => ct.GiaBan >= filterDto.MinPrice.Value);

                    if (filterDto.MaxPrice.HasValue)
                        chiTietSpQueryForDto = chiTietSpQueryForDto.Where(ct => ct.GiaBan <= filterDto.MaxPrice.Value);

                    if (!string.IsNullOrEmpty(filterDto.MaSize))
                        chiTietSpQueryForDto = chiTietSpQueryForDto.Where(ct => ct.MaSize == filterDto.MaSize);

                    if (!string.IsNullOrEmpty(filterDto.MaMau))
                        chiTietSpQueryForDto = chiTietSpQueryForDto.Where(ct => ct.MaMau == filterDto.MaMau);

                    var minPrice = chiTietSpQueryForDto.Min(ct => ct.GiaBan) ?? 0;
                    var maxPrice = chiTietSpQueryForDto.Max(ct => ct.GiaBan) ?? 0;

                    dto.MinPrice = minPrice;
                    dto.MaxPrice = maxPrice;
                }

                return result.AsQueryable();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFilteredProducts: {ex.Message}");
                return Enumerable.Empty<ProductWithPriceRangeDto>().AsQueryable();
            }
        }

        public PagedResultDto<ProductWithPriceRangeDto> GetPagedProducts(FilterProductDto filterDto, int pageNumber, int pageSize, string? sortBy)
        {
            try
            {
                var query = GetFilteredProducts(filterDto);

                // Sắp xếp theo yêu cầu
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "price_asc":
                            query = query.OrderBy(sp => sp.MinPrice);
                            break;
                        case "price_desc":
                            query = query.OrderByDescending(sp => sp.MaxPrice);
                            break;
                        case "name_asc":
                            query = query.OrderBy(sp => sp.TenSP);
                            break;
                        case "name_desc":
                            query = query.OrderByDescending(sp => sp.TenSP);
                            break;
                        default:
                            query = query.OrderBy(sp => sp.TenSP);
                            break;
                    }
                }

                // Phân trang
                var totalItems = query.Count();
                var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return new PagedResultDto<ProductWithPriceRangeDto>(items, totalItems, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPagedProducts: {ex.Message}");
                return new PagedResultDto<ProductWithPriceRangeDto>(new List<ProductWithPriceRangeDto>(), 0, pageNumber, pageSize);
            }
        }

        public MinMaxPriceDto GetMinMaxPrice()
        {
            try
            {
                var minPrice = _context.ChiTietSp.Min(ct => ct.GiaBan);
                var maxPrice = _context.ChiTietSp.Max(ct => ct.GiaBan);

                return new MinMaxPriceDto
                {
                    MinPrice = minPrice ?? 0, // Xử lý nếu minPrice hoặc maxPrice là null
                    MaxPrice = maxPrice ?? 0
                };
            }
            catch (Exception ex)
            {
                // In ra lỗi để debug
                Console.WriteLine($"Error in GetMinMaxPrice: {ex.Message}");
                return new MinMaxPriceDto { MinPrice = 0, MaxPrice = 0 }; // Trả về giá trị mặc định nếu có lỗi
            }
        }
    }
}
