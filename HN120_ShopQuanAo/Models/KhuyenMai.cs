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
        [Required(ErrorMessage = "Tên Khuyến Mại là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên Khuyến Mại không được vượt quá 100 ký tự")]
        public string TenKhuyenMai { get; set; }
        [Required(ErrorMessage = "Số phần trăm Khuyến Mại là bắt buộc")]
        [Range(0, 100, ErrorMessage = "Phần trăm giảm phải từ 0 đến 100")]
        public decimal PhanTramGiam { get; set; }
        public int? TrangThai { get; set; }
		public virtual List<ChiTietSp>? ChiTietSps { get; set; }
	}
}
