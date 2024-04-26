using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class User : IdentityUser
	{
		public string Avatar { get; set; } // Link ảnh đại diện
		public int? Point { get; set; }//Điểm tích lũy
		public int? Status { get; set; } // trạng thái : 0 == Ko còn hoạt động, ẩn nick ,....

		public virtual List<HoaDon>? HoaDons { get; set; }
		public virtual List<GioHang>? GioHangs { get; set; }
	}
}
