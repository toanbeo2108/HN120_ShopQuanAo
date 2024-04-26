using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class GioHang
	{
		[Key]
		public string MaGioHang { get; set; }
		public string? UserID { get; set; }
		public decimal? TongTien { get; set; }
		public int? MoTa { get; set; }
		public int? TrangThai { get; set; }

		public virtual User? User { get; set; }
		public virtual List<GioHangChiTiet>? GioHangChiTiets { get; set; }
	}
}
