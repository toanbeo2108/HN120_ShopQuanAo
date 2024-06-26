using HN120_ShopQuanAo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.ViewModels
{
    public class AddChiTietSpViewModel
    {
        public string MaSp { get; set; }
        public List<string> SelectedSizes { get; set; }
        public List<string> SelectedMaus { get; set; }
        public List<TempChiTietSpViewModel> TempChiTietSps { get; set; }
    }

    public class TempChiTietSpViewModel
    {
        public string MaSp { get; set; }
        public string MaSize { get; set; }
        public string TenSize { get; set; }
        public string MaMau { get; set; }
        public string TenMau { get; set; }
        public decimal DonGia { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuongTon { get; set; }
        public string MaKhuyenMai { get; set; }
        public string UrlAnhSpct { get; set; }
    }
}
