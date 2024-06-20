using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class ChatLieu
	{
		[Key] public string MaChatLieu { get; set; }
        [Required(ErrorMessage = "Tên Chất liệu là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên chất liệu không được vượt quá 100 ký tự")]
        public string TenChatLieu { get; set; }
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? MoTa { get; set; }
        public int? TrangThai { get; set; }
		public virtual List<SanPham>? SanPhams { get; set; }

	}
}
