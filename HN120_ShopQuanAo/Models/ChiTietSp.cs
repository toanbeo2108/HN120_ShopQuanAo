using System;
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
		public string? MaTheLoai { get; set; }
		public decimal? GiaNhap { get; set; }
		public decimal? GiaBan { get; set; }
		public int? SoLuongTon { get; set; }
		public int? TrangThai { get; set; }

		public virtual SanPham? SanPham { get; set; }
		public virtual TheLoai? TheLoai { get; set; }
		public virtual MauSac? MauSac { get; set; }
		public virtual Size? Size { get; set; }
		public virtual List<GioHangChiTiet>? GioHangChiTiet { get; set; }
		public virtual List<HoaDonChiTiet>? HoaDonChiTiet { get; set; }


	}
}
