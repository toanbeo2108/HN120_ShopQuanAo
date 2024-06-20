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
		public string? MaTheLoai { get; set; }
        public string? MaChatLieu { get; set; }
        [Required(ErrorMessage = "URL Avatar là bắt buộc")]
        public string? UrlAvatar { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
        public string? TenSP { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? Mota { get; set; }
        public DateTime? NgayNhap { get; set; }
        public int? TongSoLuong { get; set; }
        public int? TrangThai { get; set; }

        public virtual List<ChiTietSp>? ChiTietSps { get; set; }
        public virtual ThuongHieu? ThuongHieu { get; set; }
		public virtual TheLoai? TheLoai { get; set; }
        public virtual ChatLieu? ChatLieu { get; set; }


    }
}
