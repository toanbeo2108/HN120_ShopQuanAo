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
		public decimal? DieuKienGiam { get; set; }
		public decimal? GiaGiamToiThieu { get; set; }
		public decimal? GiaGiamToiDa { get; set; }
		public DateTime? NgayBatDau { get; set; }
		public DateTime? NgayKetThuc { get; set; }
		public int? KieuGiamGia { get; set; }
		public decimal? GiaTriGiam { get; set; }
		public int? SoLuong { get; set; }
		public int? TrangThai { get; set; }

		public virtual List<HoaDon>? HoaDons { get; set; }
		public virtual List<User_Voucher>? User_Vouchers { get; set; }
		public virtual List<VoucherHistory>? VoucherHistorys { get; set; }
	}
}
