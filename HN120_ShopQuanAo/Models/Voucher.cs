using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class Voucher
	{
		[Key]
		public string MaVoucher { get; set; }
		public string? TenVoucher { get; set; }
		public decimal? DieuKien { get; set; }
		public decimal? GiamTien { get; set; }
		public float? ChietKhau { get; set; }
		public int? TrangThai { get; set; }

		public virtual List<HoaDon>? HoaDons { get; set; }
	}
}
