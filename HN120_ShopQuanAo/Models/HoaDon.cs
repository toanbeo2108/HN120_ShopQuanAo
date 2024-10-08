﻿using HN120_ShopQuanAo.Data.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class HoaDon
	{
		[Key]
		public string MaHoaDon { get; set; }
		public string? UserID { get; set; }
		public string? MaVoucher { get; set; }
		public DateTime? NgayTaoDon { get; set; }
        public DateTime? NgayThayDoi {get; set;}
        public string? TenKhachHang { get; set; }
		public string? SoDienThoai { get; set; }
		public decimal? PhiShip { get; set; }
		public decimal? TongGiaTriHangHoa { get; set; }
		public int? PhuongThucThanhToan { get; set; }
		public string? PhanLoai { get; set; }
		public string? Ghichu { get; set; }
		//địa chỉ
		public string? TinhThanh { get; set; }
		public string? QuanHuyen { get; set; }
		public string? XaPhuong { get; set; }
		public string? Cuthe { get; set; }
		public int? TrangThai { get; set; }

		public virtual User? User { get; set; }
		public virtual Voucher? Voucher { get; set; }
		public virtual List<ThanhToan_HoaDon>? ThanhToan_HoaDons { get; set; }
		public virtual List<HoaDonChiTiet>? HoaDonChiTiets { get; set; }
		public virtual List<HoaDon_History>? HoaDon_History { get; set; }

	}
}
