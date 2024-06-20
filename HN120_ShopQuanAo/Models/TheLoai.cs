using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class TheLoai
	{
		[Key]
		public string MaTheLoai { get; set; }
        [Required(ErrorMessage = "Tên Thể loại là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên Thể Loại không được vượt quá 100 ký tự")]
        public string? TenTheLoai { get; set; }
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? MoTa { get; set; }
		public int? TrangThai { get; set; }

		public virtual List<SanPham>? SanPhams { get; set; }

	}
}
