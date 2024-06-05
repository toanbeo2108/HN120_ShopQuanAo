using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class User_Voucher
	{
		[Key] public string UserVoucherID { get; set; }
        public string? UserID { get; set; }
        public string? MaVoucher { get; set; }
        public int? TrangThai { get; set; }
		public virtual Voucher? Voucher { get; set; }
		public virtual User? User { get; set; }
	}
}
