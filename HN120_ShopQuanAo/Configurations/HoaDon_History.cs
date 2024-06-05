using HN120_ShopQuanAo.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Configurations
{
	public class HoaDon_History
	{
		[Key] public string LichSuHoaDonID { get; set; }
        public string? MaHoaDon { get; set; }
        public string? UserID { get; set; }
        public DateTime? NgayTaoDon { get; set; }
        public DateTime? NgayThayDoi { get; set; }
        public decimal? TongGiaTri { get; set; }
        public string? HinhThucThanhToan { get; set; }
        public string? ChiTiet { get; set; }
        public int? TrangThai { get; set; }

        public virtual HoaDon? HoaDon { get; set; }
	}
}
