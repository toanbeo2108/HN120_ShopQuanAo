using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class User : IdentityUser
	{
		public string Avatar { get; set; } // Link ảnh đại diện
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public int? Gender { get; set; }// Giới tính
        public DateTime? Birthday { get; set; }
        public string? CardNumber { get; set; }
        public int? Status { get; set; } // trạng thái : 0 == Ko còn hoạt động, ẩn nick ,....

		public virtual List<HoaDon>? HoaDons { get; set; }
		public virtual List<GioHang>? GioHangs { get; set; }
		public virtual List<DeliveryAddress>? DeliveryAddress { get; set; }
		public virtual List<User_Voucher>? User_Vouchers { get; set; }
	}
}
