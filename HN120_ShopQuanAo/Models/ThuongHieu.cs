﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class ThuongHieu
	{
        [Key]
        public string MaThuongHieu { get; set; }
        [Required(ErrorMessage = "Tên Thương hiệu là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên Thương hiệu không được vượt quá 100 ký tự")]
        public string? TenThuongHieu { get; set; }
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? MoTa { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<SanPham>? SanPhams { get; set; }
	}
}
