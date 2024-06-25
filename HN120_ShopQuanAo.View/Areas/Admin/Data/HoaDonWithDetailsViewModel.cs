using HN120_ShopQuanAo.Data.Models;

namespace HN120_ShopQuanAo.View.Areas.Admin.Data
{
    public class HoaDonWithDetailsViewModel
    {
        public HoaDon HoaDon { get; set; }
        public List<HoaDonChiTiet> HoaDonChiTiets { get; set; }
    }
}
