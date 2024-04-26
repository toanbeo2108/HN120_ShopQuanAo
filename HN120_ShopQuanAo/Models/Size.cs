using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class Size
	{

		[Key]
		public string MaSize { get; set; }
		public string? TenSize { get; set; }
		public string? MoTa { get; set; }
		public int? TrangThai { get; set; }

		public virtual List<ChiTietSp>? ChiTietSps { get; set; }

	}
}
