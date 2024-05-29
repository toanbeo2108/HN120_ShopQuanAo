using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class SanPham
	{
		[Key]
		public string MaSp { get; set; }
		public string? MaThuongHieu { get; set; }
		public string? UrlAvatar { get; set; }
		public string? TenSP { get; set; }
        public string? Mota { get; set; }
        public int? TongSoLuong { get; set; }
        public int? TrangThai { get; set; }

        public virtual List<ChiTietSp>? ChiTietSps { get; set; }
        public virtual ThuongHieu? ThuongHieu { get; set; }
	}
}
