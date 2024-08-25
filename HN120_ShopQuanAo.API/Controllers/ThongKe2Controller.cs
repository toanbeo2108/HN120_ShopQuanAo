using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKe2Controller : ControllerBase
    {
        private readonly AppDbContext _context;
        public ThongKe2Controller(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("doanh-thu")]
        public async Task<IActionResult> GetRevenue(DateTime? startDate, DateTime? endDate, int? year, int? month, int? day)
        {
            var validationResult = ValidateDateParameters(startDate, endDate, year, month, day);
            if (validationResult != null)
                return BadRequest(validationResult);

            var query = _context.HoaDon.AsQueryable();
            query = DateFilterHelper.ApplyDateFilter(query, startDate, endDate, year, month, day);
            var revenue = await query.SumAsync(hd => hd.TongGiaTriHangHoa ?? 0);
            return Ok(revenue);
        }
        [HttpGet("doanh-thu-theo-ngay")]
        public async Task<IActionResult> GetDailyRevenue(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate == null || toDate == null)
            {
                return BadRequest("fromDate and toDate are required.");
            }

            toDate = toDate.Value.AddDays(1).AddTicks(-1);

            var doanhThu = await _context.HoaDon
                .Where(hd => hd.NgayTaoDon >= fromDate && hd.NgayTaoDon <= toDate)
                .GroupBy(hd => hd.NgayTaoDon.Value.Date)
                .Select(g => new
                {
                    Ngay = g.Key,
                    TongDoanhThu = g.Sum(hd => hd.TongGiaTriHangHoa)
                })
                .ToListAsync();

            var doanhThuDict = doanhThu.ToDictionary(dt => dt.Ngay, dt => dt.TongDoanhThu);

            var result = Enumerable.Range(0, (toDate.Value.Date - fromDate.Value.Date).Days + 1)
                .Select(i => fromDate.Value.Date.AddDays(i))
                .Select(date => new
                {
                    Ngay = date,
                    TongDoanhThu = doanhThuDict.ContainsKey(date) ? doanhThuDict[date] : 0
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("doanh-thu-theo-thang")]
        public async Task<IActionResult> GetMonthlyRevenue(DateTime? startDate, DateTime? endDate, int? year, int? month, int? day)
        {
            var validationResult = ValidateDateParameters(startDate, endDate, year, month, day);
            if (validationResult != null)
                return BadRequest(validationResult);

            var query = _context.HoaDon.AsQueryable();
            query = DateFilterHelper.ApplyDateFilter(query, startDate, endDate, year, month, day);

            var doanhThu = await query
                .GroupBy(hd => new { hd.NgayTaoDon.Value.Year, hd.NgayTaoDon.Value.Month })
                .Select(g => new
                {
                    Nam = g.Key.Year,
                    Thang = g.Key.Month,
                    TongDoanhThu = g.Sum(hd => hd.TongGiaTriHangHoa ?? 0)
                })
                .ToListAsync();

            var fromDate = startDate ?? new DateTime(year ?? 1, month ?? 1, 1);
            var toDate = endDate ?? DateTime.Now;

            var result = Enumerable.Range(0, (toDate.Year - fromDate.Year) * 12 + toDate.Month - fromDate.Month + 1)
                .Select(i => fromDate.AddMonths(i))
                .Select(date => new
                {
                    Nam = date.Year,
                    Thang = date.Month,
                    TongDoanhThu = doanhThu.FirstOrDefault(dt => dt.Nam == date.Year && dt.Thang == date.Month)?.TongDoanhThu ?? 0
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("doanh-thu-theo-nam")]
        public async Task<IActionResult> GetYearlyRevenue(DateTime? startDate, DateTime? endDate, int? year, int? month, int? day)
        {
            var validationResult = ValidateDateParameters(startDate, endDate, year, month, day);
            if (validationResult != null)
                return BadRequest(validationResult);

            var query = _context.HoaDon.AsQueryable();
            query = DateFilterHelper.ApplyDateFilter(query, startDate, endDate, year, month, day);

            var doanhThu = await query
                .GroupBy(hd => hd.NgayTaoDon.Value.Year)
                .Select(g => new
                {
                    Nam = g.Key,
                    TongDoanhThu = g.Sum(hd => hd.TongGiaTriHangHoa ?? 0)
                })
                .ToListAsync();

            var fromDate = startDate ?? new DateTime(year ?? 1, 1, 1);
            var toDate = endDate ?? DateTime.Now;

            var result = Enumerable.Range(fromDate.Year, toDate.Year - fromDate.Year + 1)
                .Select(y => new
                {
                    Nam = y,
                    TongDoanhThu = doanhThu.FirstOrDefault(dt => dt.Nam == y)?.TongDoanhThu ?? 0
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("hoa-don-chi-tiet")]
        public async Task<IActionResult> GetInvoiceDetails(DateTime? startDate, DateTime? endDate, int? year, int? month, int? day)
        {
            // Validate the date parameters
            var validationResult = ValidateDateParameters(startDate, endDate, year, month, day);
            if (validationResult != null)
                return BadRequest(validationResult);

            // Get all invoice details with optional filtering by date
            var query = _context.HoaDonChiTiet.Include(hdct => hdct.HoaDon).AsQueryable();
            query = DateFilterHelper.ApplyDateFilter(query, startDate, endDate, year, month, day);

            var invoiceDetails = await query.ToListAsync();
            return Ok(invoiceDetails);
        }

        [HttpGet("so-luong-san-pham")]
        public async Task<IActionResult> GetProductCount(DateTime? startDate, DateTime? endDate, int? year, int? month, int? day)
        {
            var validationResult = ValidateDateParameters(startDate, endDate, year, month, day);
            if (validationResult != null)
                return BadRequest(validationResult);

            // Lọc hóa đơn dựa trên các điều kiện ngày tháng năm
            var hoaDonQuery = _context.HoaDon.AsQueryable();
            hoaDonQuery = DateFilterHelper.ApplyDateFilter(hoaDonQuery, startDate, endDate, year, month, day);

            // Lấy danh sách mã hóa đơn
            var hoaDonIds = await hoaDonQuery.Select(hd => hd.MaHoaDon).ToListAsync();

            // Lọc chi tiết hóa đơn dựa trên danh sách mã hóa đơn
            var hoaDonChiTietQuery = _context.HoaDonChiTiet
                                             .Where(hdct => hoaDonIds.Contains(hdct.MaHoaDon));

            // Tính tổng số lượng sản phẩm đã bán
            var totalProductCount = await hoaDonChiTietQuery.SumAsync(hdct => hdct.SoLuongMua ?? 0);
            return Ok(totalProductCount);
        }

        [HttpGet("so-luong-hoa-don")]
        public async Task<IActionResult> GetInvoiceCount(DateTime? startDate, DateTime? endDate, int? year, int? month, int? day)
        {
            var validationResult = ValidateDateParameters(startDate, endDate, year, month, day);
            if (validationResult != null)
                return BadRequest(validationResult);

            var query = _context.HoaDon.AsQueryable();
            query = DateFilterHelper.ApplyDateFilter(query, startDate, endDate, year, month, day);
            var invoiceCount = await query.CountAsync();
            return Ok(invoiceCount);
        }
        [HttpGet("top-selling-product")]
        public async Task<IActionResult> GetTopSellingProduct(DateTime? startDate, DateTime? endDate, int? year, int? month, int? day, int top = 5)
        {
            try
            {
                // Lọc danh sách hóa đơn dựa trên các điều kiện ngày tháng
                var invoicesQuery = _context.HoaDon.AsQueryable();
                invoicesQuery = DateFilterHelper.ApplyDateFilter(invoicesQuery, startDate, endDate, year, month, day);

                // Lấy danh sách chi tiết hóa đơn từ danh sách hóa đơn
                var invoiceDetailsQuery = _context.HoaDonChiTiet
                                                  .Where(hdct => invoicesQuery.Select(hd => hd.MaHoaDon).Contains(hdct.MaHoaDon));

                // Nhóm theo SKU và tính tổng số lượng mua
                var topSellingProducts = await invoiceDetailsQuery
                    .GroupBy(hdct => hdct.SKU)
                    .Select(g => new TopSellingProductSimpleDto
                    {
                        SKU = g.Key,
                        TotalQuantity = g.Sum(hdct => hdct.SoLuongMua) ?? 0
                    })
                    .OrderByDescending(g => g.TotalQuantity)
                    .Take(top)
                    .ToListAsync();

                // Xử lý trường hợp không có dữ liệu
                if (topSellingProducts == null || !topSellingProducts.Any())
                {
                    return Ok(new List<TopSellingProductSimpleDto>());
                }

                // Trả về danh sách top sản phẩm bán chạy nhất
                return Ok(topSellingProducts);
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về lỗi 500
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("top-selling-product2")]
        public async Task<IActionResult> GetTopSellingProduct2(DateTime? startDate, DateTime? endDate, int? year, int? month, int? day, int top = 5)
        {
            try
            {
                // Bước 1: Lọc danh sách hóa đơn dựa trên các điều kiện ngày tháng
                var invoicesQuery = _context.HoaDon.AsQueryable();
                invoicesQuery = DateFilterHelper.ApplyDateFilter(invoicesQuery, startDate, endDate, year, month, day);

                // Bước 2: Lấy danh sách chi tiết hóa đơn từ danh sách hóa đơn
                var invoiceDetailsQuery = _context.HoaDonChiTiet
                    .Where(hdct => invoicesQuery.Select(hd => hd.MaHoaDon).Contains(hdct.MaHoaDon));

                // Bước 3: Nhóm theo SKU và tính tổng số lượng mua
                var topSellingProducts = await invoiceDetailsQuery
                    .GroupBy(hdct => hdct.SKU)
                    .Select(g => new
                    {
                        SKU = g.Key,
                        TotalQuantity = g.Sum(hdct => hdct.SoLuongMua) ?? 0
                    })
                    .OrderByDescending(g => g.TotalQuantity)
                    .Take(top)
                    .ToListAsync();

                // Bước 4: Lấy thông tin từ bảng ChiTietSp
                var chiTietSpList = await _context.ChiTietSp
                    .Where(ct => topSellingProducts.Select(p => p.SKU).Contains(ct.SKU))
                    .ToListAsync();

                // Bước 5: Kết hợp thông tin từ bảng SanPham
                var result = new List<TopSellingProductSimpleDto2>();
                foreach (var product in topSellingProducts)
                {
                    var chiTietSp = chiTietSpList.FirstOrDefault(ct => ct.SKU == product.SKU);
                    if (chiTietSp != null)
                    {
                        var sanPham = await _context.SanPham
                            .FirstOrDefaultAsync(sp => sp.MaSp == chiTietSp.MaSp);

                        var dto = new TopSellingProductSimpleDto2
                        {
                            SKU = chiTietSp.SKU,
                            MaSp = chiTietSp.MaSp,
                            TotalQuantity = product.TotalQuantity,
                            TenSP = sanPham?.TenSP,
                            DonGia = chiTietSp.DonGia,
                            SoLuongTon = chiTietSp.SoLuongTon
                        };
                        result.Add(dto);
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về lỗi 500
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
            
        }


        [HttpGet("san-pham-sap-het-hang")]
        public async Task<IActionResult> GetLowStockProducts(int threshold = 10)
        {
            var lowStockProducts = await _context.ChiTietSp
                .Where(sp => sp.SoLuongTon < threshold)
                .ToListAsync();

            return Ok(lowStockProducts);
        }

        [HttpGet("tang-truong-doanh-thu")]
        public async Task<IActionResult> GetRevenueGrowth(int? year, int? month)
        {
            if (year.HasValue && month.HasValue)
            {
                var currentMonthRevenue = await _context.HoaDon
                    .Where(hd => hd.NgayTaoDon.Value.Year == year.Value && hd.NgayTaoDon.Value.Month == month.Value)
                    .SumAsync(hd => hd.TongGiaTriHangHoa ?? 0);

                var previousMonthRevenue = await _context.HoaDon
                    .Where(hd => hd.NgayTaoDon.Value.Year == (month.Value == 1 ? year.Value - 1 : year.Value) &&
                                 hd.NgayTaoDon.Value.Month == (month.Value == 1 ? 12 : month.Value - 1))
                    .SumAsync(hd => hd.TongGiaTriHangHoa ?? 0);

                var growth = previousMonthRevenue == 0 ? 0 : ((currentMonthRevenue - previousMonthRevenue) / previousMonthRevenue) * 100;
                return Ok(new { growth });
            }
            else if (year.HasValue)
            {
                var currentYearRevenue = await _context.HoaDon
                    .Where(hd => hd.NgayTaoDon.Value.Year == year.Value)
                    .SumAsync(hd => hd.TongGiaTriHangHoa ?? 0);

                var previousYearRevenue = await _context.HoaDon
                    .Where(hd => hd.NgayTaoDon.Value.Year == year.Value - 1)
                    .SumAsync(hd => hd.TongGiaTriHangHoa ?? 0);

                var growth = previousYearRevenue == 0 ? 0 : ((currentYearRevenue - previousYearRevenue) / previousYearRevenue) * 100;
                return Ok(new { growth });
            }
            return BadRequest("Cần cung cấp thông số năm hoặc tháng.");
        }

        private string ValidateDateParameters(DateTime? startDate, DateTime? endDate, int? year, int? month, int? day)
        {
            if (startDate.HasValue || endDate.HasValue)
            {
                if (year.HasValue || month.HasValue || day.HasValue)
                    return "Không thể sử dụng cả khoảng thời gian và các thông số ngày, tháng, năm cùng một lúc.";
                if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                    return "Ngày bắt đầu không thể lớn hơn ngày kết thúc.";
            }
            else if (year.HasValue || month.HasValue || day.HasValue)
            {
                if (day.HasValue && (!year.HasValue || !month.HasValue))
                    return "Ngày yêu cầu cần có cả năm và tháng.";
                if (month.HasValue && !year.HasValue)
                    return "Tháng yêu cầu cần có năm.";
            }
            return null;
        }
    }

    // File path: Helpers/DateFilterHelper.cs
    public static class DateFilterHelper
    {
        public static IQueryable<HoaDon> ApplyDateFilter(IQueryable<HoaDon> query, DateTime? startDate, DateTime? endDate, int? year, int? month, int? day)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(hd => hd.NgayTaoDon >= startDate && hd.NgayTaoDon <= endDate);
            }
            else if (year.HasValue && month.HasValue && day.HasValue)
            {
                var date = new DateTime(year.Value, month.Value, day.Value);
                query = query.Where(hd => hd.NgayTaoDon.Value.Date == date.Date);
            }
            else if (year.HasValue && month.HasValue)
            {
                query = query.Where(hd => hd.NgayTaoDon.Value.Year == year && hd.NgayTaoDon.Value.Month == month);
            }
            else if (year.HasValue)
            {
                query = query.Where(hd => hd.NgayTaoDon.Value.Year == year);
            }

            return query;
        }

        public static IQueryable<HoaDonChiTiet> ApplyDateFilter(
        IQueryable<HoaDonChiTiet> query,
        DateTime? startDate, DateTime? endDate, int? year, int? month, int? day)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(hdct => hdct.HoaDon.NgayTaoDon >= startDate && hdct.HoaDon.NgayTaoDon <= endDate);
                Console.WriteLine($"Filtered by date range: {startDate} - {endDate}");
            }
            else if (year.HasValue && month.HasValue && day.HasValue)
            {
                var date = new DateTime(year.Value, month.Value, day.Value);
                query = query.Where(hdct => hdct.HoaDon.NgayTaoDon.Value.Date == date.Date);
                Console.WriteLine($"Filtered by specific date: {date}");
            }
            else if (year.HasValue && month.HasValue)
            {
                query = query.Where(hdct => hdct.HoaDon.NgayTaoDon.Value.Year == year && hdct.HoaDon.NgayTaoDon.Value.Month == month);
                Console.WriteLine($"Filtered by year and month: {year}-{month}");
            }
            else if (year.HasValue)
            {
                query = query.Where(hdct => hdct.HoaDon.NgayTaoDon.Value.Year == year);
                Console.WriteLine($"Filtered by year: {year}");
            }

            // Print out the query for debugging purposes
            Console.WriteLine($"Resulting query: {query.ToQueryString()}");

            return query;
        }
    }
    public class TopSellingProductSimpleDto
    {
        public string SKU { get; set; }
        public int TotalQuantity { get; set; }
    }
    public class TopSellingProductSimpleDto2
    {
        public string SKU { get; set; }
        public string MaSp { get; set; }
        public string TenSP { get; set; }
        public decimal? DonGia { get; set; }
        public int? SoLuongTon { get; set; }
        public int TotalQuantity { get; set; }
    }
}
