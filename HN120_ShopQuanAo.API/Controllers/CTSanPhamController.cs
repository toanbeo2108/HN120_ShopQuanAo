using HN120_ShopQuanAo.API.Data;
using HN120_ShopQuanAo.API.IResponsitories;
using HN120_ShopQuanAo.API.Responsitories;
using HN120_ShopQuanAo.Data.Models;
using HN120_ShopQuanAo.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/CTSanPham")]
    [ApiController]
    public class CTSanPhamController : ControllerBase
    {

        private readonly IAllResponsitories<ChiTietSp> _iresponCTSP;
        private readonly IAllResponsitories<SanPham> _iresponSP;
        private readonly IAllResponsitories<KhuyenMai> _iresponKM;


        AppDbContext _context = new AppDbContext();
        public CTSanPhamController()
        {
            _iresponCTSP = new AllResponsitories<ChiTietSp>(_context, _context.ChiTietSp);
            _iresponSP = new AllResponsitories<SanPham>(_context, _context.SanPham);
            _iresponKM = new AllResponsitories<KhuyenMai>(_context, _context.KhuyenMai);

        }
        [HttpPost("[Action]")]
        public async Task<bool> AddLstCTSP([FromBody] List<ChiTietSp> lstctsp)
        {
            try
            {
                var groupedByProduct = new Dictionary<string, int>();

                foreach (var item in lstctsp)
                {
                    item.SKU = item.MaSp + item.MaSize + item.MaMau;
                    item.TrangThai = 1;

                    // Tính toán Giá Bán
                    item.GiaBan = item.DonGia;
                    if (item.MaKhuyenMai != null)
                    {
                        var km = await _iresponKM.GetByID(item.MaKhuyenMai);
                        if (km != null)
                        {
                            item.GiaBan = item.DonGia - (item.DonGia * km.PhanTramGiam / 100);
                        }
                    }

                    await _iresponCTSP.CreateItem(item);

                    // Tính tổng số lượng tồn
                    if (groupedByProduct.ContainsKey(item.MaSp))
                    {
                        groupedByProduct[item.MaSp] += item.SoLuongTon.GetValueOrDefault(0);
                    }
                    else
                    {
                        groupedByProduct[item.MaSp] = item.SoLuongTon.GetValueOrDefault(0);
                    }
                }

                // Cập nhật tổng số lượng cho sản phẩm
                foreach (var kvp in groupedByProduct)
                {
                    var sp = await _iresponSP.GetByID(kvp.Key);
                    if (sp != null)
                    {
                        sp.TongSoLuong = kvp.Value;
                        await _iresponSP.UpdateItem(sp);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetByMaSp")]
        public async Task<IActionResult> GetByMaSp(string maSp)
        {
            try
            {
                var chiTietSps = await _iresponCTSP.GetAll();
                var result = chiTietSps.Where(ct => ct.MaSp == maSp).ToList();
                return Ok(result); // Trả về danh sách
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("UpdateChiTietSp")]
        public async Task<IActionResult> UpdateChiTietSp([FromBody] List<ChiTietSp> chiTietSps)
        {
            try
            {
                var groupedByProduct = new Dictionary<string, int>();

                foreach (var ctsp in chiTietSps)
                {
                    // Cập nhật thông tin chi tiết sản phẩm
                    var existingCtsp = await _iresponCTSP.GetByID(ctsp.SKU);
                    if (existingCtsp != null)
                    {
                        existingCtsp.DonGia = ctsp.DonGia;
                        existingCtsp.GiaBan = ctsp.DonGia;
                        existingCtsp.SoLuongTon = ctsp.SoLuongTon;
                        existingCtsp.UrlAnhSpct = ctsp.UrlAnhSpct;
                        existingCtsp.MaKhuyenMai = ctsp.MaKhuyenMai;



                        await _iresponCTSP.UpdateItem(existingCtsp);

                        // Tính tổng số lượng tồn
                        if (groupedByProduct.ContainsKey(existingCtsp.MaSp))
                        {
                            groupedByProduct[existingCtsp.MaSp] += existingCtsp.SoLuongTon.GetValueOrDefault(0);
                        }
                        else
                        {
                            groupedByProduct[existingCtsp.MaSp] = existingCtsp.SoLuongTon.GetValueOrDefault(0);
                        }
                    }
                }

                // Cập nhật tổng số lượng cho sản phẩm
                foreach (var kvp in groupedByProduct)
                {
                    var sp = await _iresponSP.GetByID(kvp.Key);
                    if (sp != null)
                    {
                        sp.TongSoLuong = kvp.Value;
                        await _iresponSP.UpdateItem(sp);
                    }
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("[Action]")]
        public async Task<IEnumerable<ChiTietSp>> GetAllCTSanPham()
        {
            return await _iresponCTSP.GetAll();
        }
        [HttpGet("[Action]")]
        public async Task<ChiTietSp> GetCTSPById(string id)
        {
            return await _iresponCTSP.GetByID(id);
        }
        [HttpPost("[Action]")]
        public async Task<bool> AddCTSP(string? MaSp, string? MaSize, string? MaMau, string? MaKhuyenMai, string? UrlAnhSpct, decimal? DonGia, int? SoLuongTon)
        {
            var lstsp = await _iresponSP.GetAll();
            var sp = lstsp.FirstOrDefault(c => c.MaSp == MaSp);
            var lstkm = await _iresponKM.GetAll();
            var km = lstkm.FirstOrDefault(c => c.MaKhuyenMai == MaKhuyenMai);
            decimal? ptkm = km.PhanTramGiam;
            var ctsp = await _iresponCTSP.GetAll();
            var lstspct = ctsp.Where(c => c.MaSp == sp.MaSp && c.TrangThai == 1);
            ChiTietSp b = new ChiTietSp();
            b.SKU = MaSp + MaMau + MaSize;
            b.MaSp = MaSp;
            b.MaMau = MaMau;
            b.MaSize = MaSize;
            b.MaKhuyenMai = MaKhuyenMai;
            b.UrlAnhSpct = UrlAnhSpct;
            b.DonGia = DonGia;
            if (b.MaKhuyenMai == null || ptkm == 0)
            {
                b.GiaBan = DonGia;
            }
            else
            {
                b.GiaBan = DonGia - (DonGia * ptkm / 100);
            }

            b.SoLuongTon = SoLuongTon;
            if (SoLuongTon == 0)
            {
                b.TrangThai = 0;
            }
            if (SoLuongTon > 0)
            {
                b.TrangThai = 1;
            }
            var check = await _iresponCTSP.CreateItem(b);
            sp.TongSoLuong = sp.TongSoLuong + SoLuongTon;
            var check2 = await _iresponSP.UpdateItem(sp);
            return check;
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> EditCTSP(string id, ChiTietSp _ctsp)
        {

            var ctsp = await _iresponCTSP.GetAll();
            var b = ctsp.FirstOrDefault(c => c.SKU == id);


            if (b != null)
            {
                b.MaKhuyenMai = _ctsp.MaKhuyenMai;
                b.UrlAnhSpct = _ctsp.UrlAnhSpct;
                b.DonGia = _ctsp.DonGia;
                b.GiaBan = _ctsp.DonGia;

                b.SoLuongTon = _ctsp.SoLuongTon;
                if (b.SoLuongTon == 0)
                {
                    b.TrangThai = 0;
                }
                if (b.SoLuongTon > 0)
                {
                    b.TrangThai = 1;
                }

                await _iresponCTSP.UpdateItem(b);
                var lstsp = await _iresponSP.GetAll();
                var sp = lstsp.FirstOrDefault(c => c.MaSp == _ctsp.MaSp);
                var lstspct = ctsp.Where(c => c.MaSp == sp.MaSp && c.TrangThai == 1);
                sp.TongSoLuong = lstspct.Sum(c => c.SoLuongTon);
                return await _iresponSP.UpdateItem(sp);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateStatusCTSanPham(string id, int? _ctsp)
        {
            var ctsp = await _iresponCTSP.GetAll();
            var b = ctsp.FirstOrDefault(c => c.SKU == id);
            if (b != null)
            {
                b.TrangThai = _ctsp;
                return await _iresponCTSP.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> deleteCTSP(string id)
        {
            var listBook = await _iresponCTSP.GetAll();
            var re = listBook.FirstOrDefault(c => c.SKU == id);
            if (re != null)
            {
                return await _iresponCTSP.DeleteItem(re);
            }
            else
            {
                return false;
            }
        }
    }
}
