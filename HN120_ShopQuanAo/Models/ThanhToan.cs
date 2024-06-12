using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class ThanhToan
	{
		[Key] 
		public string MaPhuongThuc { get; set; }
		public string? TenPhuongThuc { get; set;}
		public string? MoTa { get; set;}
		public DateTime? NgayTao { get; set;}
		public DateTime? NgayThayDoi { get; set;}
		public int? TrangThai { get; set;}

		public virtual List<ThanhToan_HoaDon>? ThanhToan_HoaDon { get; set; }
	}
}
