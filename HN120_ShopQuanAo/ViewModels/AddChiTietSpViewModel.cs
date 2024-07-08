using HN120_ShopQuanAo.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.ViewModels
{
    public class AddChiTietSpViewModel
    {
    public string MaSp { get; set; }
    public List<ChiTietSpInputModel> ChiTietSps { get; set; } = new List<ChiTietSpInputModel>();
    }

public class ChiTietSpInputModel
{
    public string SKU { get; set; }
    public string MaSize { get; set; }
    public string MaMau { get; set; }
    public decimal? DonGia { get; set; }
    //public decimal? GiaBan { get; set; }
    public int? SoLuongTon { get; set; }
    public string UrlAnhSpct { get; set; }
    public string? MaKhuyenMai { get; set; }
}
}

