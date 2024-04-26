using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class MauSac
	{
		[Key]
		public string MaMau { get; set; }
		public string? TenMau { get; set; }
		public string? MoTa { get; set; }
		public int? TrangThai { get; set; }

		public virtual List<ChiTietSp>? ChiTietSps { get; set; }
	}
}
