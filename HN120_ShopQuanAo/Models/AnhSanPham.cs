using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class AnhSanPham
	{
		[Key] public string MaAnh { get; set; }
        public string? MaSP_MaSPCT { get; set; }
        public string? TenAnh { get; set; }
        public string? Url { get; set; }
        public int? TrangThai { get; set; }
	}
}
