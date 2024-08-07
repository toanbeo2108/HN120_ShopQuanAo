﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
    public class ChiTietSp
    {
        [Key]
        public string SKU { get; set; }
        public string? MaSp { get; set; }

        public string? MaSize { get; set; }

        public string? MaMau { get; set; }

        public string? MaKhuyenMai { get; set; }

        public string? UrlAnhSpct { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá không hợp lệ")]
        public decimal? DonGia { get; set; } 

        [Range(0, double.MaxValue, ErrorMessage = "Giá bán không hợp lệ")]
        public decimal? GiaBan { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không hợp lệ")]
        public int? SoLuongTon { get; set; }

        public int? TrangThai { get; set; }


        public virtual SanPham? SanPham { get; set; }
        public virtual Size? Size { get; set; }
        public virtual MauSac? MauSac { get; set; }
        public virtual KhuyenMai? KhuyenMai { get; set; }
        //public virtual List<AnhSanPhamChiTiet>? AnhSanPhamChiTiets { get; set; }
        public virtual List<GioHangChiTiet>? GioHangChiTiet { get; set; }
        public virtual List<HoaDonChiTiet>? HoaDonChiTiet { get; set; }


    }
}
