using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Model;
using HN120_ShopQuanAo.Data.Models;

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
        public IQueryable<SanPham> GetFilteredProducts(FilterProductDto filterDto)
        {
            try
            {
                // Truy vấn ChiTietSp trước
                var chiTietQuery = _context.ChiTietSp.AsQueryable();

                if (filterDto.MinPrice.HasValue)
                    chiTietQuery = chiTietQuery.Where(ct => ct.GiaBan >= filterDto.MinPrice.Value);

                if (filterDto.MaxPrice.HasValue)
                    chiTietQuery = chiTietQuery.Where(ct => ct.GiaBan <= filterDto.MaxPrice.Value);

                if (!string.IsNullOrEmpty(filterDto.MaSize))
                    chiTietQuery = chiTietQuery.Where(ct => ct.MaSize == filterDto.MaSize);

                if (!string.IsNullOrEmpty(filterDto.MaMau))
                    chiTietQuery = chiTietQuery.Where(ct => ct.MaMau == filterDto.MaMau);

                // Lấy danh sách MaSp từ ChiTietSp sau khi lọc
                var filteredMaSp = chiTietQuery.Select(ct => ct.MaSp).Distinct();

                // Truy vấn SanPham dựa trên MaSp và các điều kiện khác
                var sanPhamQuery = _context.SanPham.Where(sp => filteredMaSp.Contains(sp.MaSp));

                if (!string.IsNullOrEmpty(filterDto.MaThuongHieu))
                    sanPhamQuery = sanPhamQuery.Where(sp => sp.MaThuongHieu == filterDto.MaThuongHieu);

                if (!string.IsNullOrEmpty(filterDto.MaTheLoai))
                    sanPhamQuery = sanPhamQuery.Where(sp => sp.MaTheLoai == filterDto.MaTheLoai);

                if (!string.IsNullOrEmpty(filterDto.MaChatLieu))
                    sanPhamQuery = sanPhamQuery.Where(sp => sp.MaChatLieu == filterDto.MaChatLieu);

                return sanPhamQuery;
            }
            catch (Exception ex)
            {
                // In ra lỗi để dễ dàng debug
                Console.WriteLine($"Error in GetFilteredProducts: {ex.Message}");
                return Enumerable.Empty<SanPham>().AsQueryable(); // Trả về một danh sách trống nếu có lỗi
            }
        }

        public PagedResultDto<SanPham> GetPagedProducts(FilterProductDto filterDto, int pageNumber, int pageSize, string? sortBy)
        {
            try
            {
                var query = GetFilteredProducts(filterDto);

                // Sắp xếp nếu cần thiết
                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "price_asc":
                            query = query.OrderBy(sp => sp.ChiTietSps.Min(ct => ct.GiaBan));
                            break;
                        case "price_desc":
                            query = query.OrderByDescending(sp => sp.ChiTietSps.Min(ct => ct.GiaBan));
                            break;
                        case "name_asc":
                            query = query.OrderBy(sp => sp.TenSP);
                            break;
                        case "name_desc":
                            query = query.OrderByDescending(sp => sp.TenSP);
                            break;
                        default:
                            query = query.OrderBy(sp => sp.TenSP); // Sắp xếp mặc định theo tên sản phẩm
                            break;
                    }
                }

                // Phân trang
                var totalItems = query.Count();
                var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return new PagedResultDto<SanPham>(items, totalItems, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                // In ra lỗi để dễ dàng debug
                Console.WriteLine($"Error in GetPagedProducts: {ex.Message}");
                return new PagedResultDto<SanPham>(new List<SanPham>(), 0, pageNumber, pageSize); // Trả về kết quả rỗng nếu có lỗi
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
