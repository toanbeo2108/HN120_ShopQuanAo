using HN120_ShopQuanAo.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ThongKeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ThongKeHoaDon")]
        public async Task<IActionResult> ThongKeHoaDon(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.HoaDon.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.NgayTaoDon >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                toDate = toDate.Value.AddDays(1).AddTicks(-1); // TRừ 1 giây để thời gian là 23:59:59 để lấy doanh thu ngày
                query = query.Where(x => x.NgayTaoDon <= toDate.Value);
            }

            var result = await query
                .GroupBy(x => 1)
                .Select(g => new
                {
                    SoLuongHoaDon = g.Count(),
                    TongGiaTriHoaDon = g.Sum(x => x.TongGiaTriHangHoa ?? 0),
                }).FirstOrDefaultAsync();

            return Ok(result);
        }
        [HttpGet("SoLuongSanPhamBanDuocTheoNgay")]
        public async Task<IActionResult> SoLuongSanPhamBanDuocTheoNgay(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate == null || toDate == null)
            {
                return BadRequest("fromDate and toDate are required.");
            }

            toDate = toDate.Value.AddDays(1).AddTicks(-1); // Include the end of the day

            var soLuongSanPham = await _context.HoaDonChiTiet
                .Where(hdct => hdct.HoaDon.NgayTaoDon >= fromDate && hdct.HoaDon.NgayTaoDon <= toDate)
                .GroupBy(hdct => hdct.HoaDon.NgayTaoDon.Value.Date)
                .Select(g => new
                {
                    Ngay = g.Key,
                    SoLuongSanPham = g.Sum(hdct => hdct.SoLuongMua)
                })
                .ToListAsync();

            return Ok(soLuongSanPham);
        }
        [HttpGet("ThongKeTop10SanPhamBanChay")]
        public async Task<IActionResult> ThongKeTop10SanPhamBanChay(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.HoaDonChiTiet
                .Include(x => x.HoaDon)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.HoaDon.NgayTaoDon >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                toDate = toDate.Value.AddDays(1).AddTicks(-1); 
                query = query.Where(x => x.HoaDon.NgayTaoDon <= toDate.Value);
            }

            var result = await query
                .GroupBy(x => new { x.SKU})
                .Select(g => new
                {
                    SKU = g.Key.SKU,
                    SoLuongBanRa = g.Sum(x => x.SoLuongMua ?? 0),
                    TongGiaTriBanRa = g.Sum(x => x.DonGia * x.SoLuongMua) ?? 0,
                })
                .OrderByDescending(x => x.SoLuongBanRa)
                .Take(10)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("ThongKeDoanhThu")]
        public async Task<IActionResult> ThongKeDoanhThu(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.HoaDon.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.NgayTaoDon >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                toDate = toDate.Value.AddDays(1).AddTicks(-1); // Include the end of the day
                query = query.Where(x => x.NgayTaoDon <= toDate.Value);
            }

            var result = await query
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TongDoanhThu = g.Sum(x => x.TongGiaTriHangHoa ?? 0) + g.Sum(x => x.PhiShip ?? 0),
                }).FirstOrDefaultAsync();

            return Ok(result);
        }

        [HttpGet("ThongKeDoanhThuTheoNgay")]
        public async Task<IActionResult> ThongKeDoanhThuTheoNgay(DateTime? fromDate, DateTime? toDate)
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

        [HttpGet("ThongKeDoanhThu7NgayGanNhat")]
        public async Task<IActionResult> ThongKeDoanhThu7NgayGanNhat()
        {
            DateTime toDate = DateTime.Now.Date;
            DateTime fromDate = toDate.AddDays(-6);

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

            var result = Enumerable.Range(0, (toDate - fromDate).Days + 1)
                .Select(i => fromDate.AddDays(i))
                .Select(date => new
                {
                    Ngay = date,
                    TongDoanhThu = doanhThuDict.ContainsKey(date) ? doanhThuDict[date] : 0
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("TongDoanhThu")]
        public async Task<IActionResult> TongDoanhThu()
        {
            var result = await _context.HoaDon
                .SumAsync(x => x.TongGiaTriHangHoa ?? 0 + x.PhiShip ?? 0);

            return Ok(result);
        }

        [HttpGet("TongSanPhamBanDuoc")]
        public async Task<IActionResult> TongSanPhamBanDuoc()
        {
            var result = await _context.HoaDonChiTiet
                .SumAsync(x => x.SoLuongMua ?? 0);

            return Ok(result);
        }

        [HttpGet("TongSoLuongHoaDon")]
        public async Task<IActionResult> TongSoLuongHoaDon()
        {
            var result = await _context.HoaDon
                .CountAsync();

            return Ok(result);
        }
    }


}

