using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class GioHangChiTiet	
	{
		[Key]

		public string MaGioHangChiTiet { get; set; }
		public string? MaGioHang { get; set; }
		public string? SKU { get; set; }
		public string? TenSp { get; set; }
		public decimal? DonGia { get; set; }
		public int? SoLuong { get; set; }
		public decimal? ThanhTien	{ get; set; }
		public int? TrangThai { get; set; }
		
		public virtual GioHang? GioHang { get; set; }
		public virtual ChiTietSp? ChiTietSps { get; set; }
	}
}
