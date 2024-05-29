using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class KhuyenMai
	{
		[Key] public string MaKhuyenMai { get; set; }
        public string? TenKhuyenMai { get; set; }
        public float? PhanTramGiam { get; set; }
        public int? TrangThai { get; set; }
		public virtual List<ChiTietSp>? ChiTietSps { get; set; }
	}
}
