﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class Voucher
	{
        [Key]
        public string MaVoucher { get; set; }
        [Required(ErrorMessage = "mời bạn nhập Tên")]
        [StringLength(40, ErrorMessage = "Tên không được quá 40 kí tự")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "mời bạn chọn kiểu giảm giá ")]
        public int? KieuGiamGia { get; set; } //1 là giảm theo %, 0 là giảm thẳng giá tiền
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        public decimal? GiaGiamToiThieu { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        public decimal? GiaGiamToiDa { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        public DateTime? NgayBatDau { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        public DateTime? NgayKetThuc { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        public decimal? GiaTriGiam { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        public int? SoLuong { get; set; }
        public string? MoTa { get; set; }
        public int? TrangThai { get; set; }

        public virtual List<HoaDon>? HoaDons { get; set; }
		public virtual List<User_Voucher>? User_Vouchers { get; set; }
		public virtual List<VoucherHistory>? VoucherHistorys { get; set; }
	}
}
