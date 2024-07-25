using HN120_ShopQuanAo.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.ViewModels
{
    public class AddSpViewModel
    {
        public string TenSp { get; set; }
        public string MaTheLoai { get; set; }
        public string MaThuongHieu { get; set; }
        public string MaChatLieu { get; set; }
        public string UrlAvatar { get; set; } // URL ảnh đại diện

        public List<ChiTietSpInputModel> ChiTietSps { get; set; } = new List<ChiTietSpInputModel>();
    }

    public class ChiTietSpInputModel
    {
        public string MaSize { get; set; }
        public string MaMau { get; set; }
        public string UrlAnhSpct { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuongTon { get; set; }
    }
}

