using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class ThanhToan_HoaDon
	{
		[Key] public string MaPhuongThuc_HoaDon { get; set; }
        public string? MaHoaDon { get; set; }
        public string? MaPhuongThuc { get; set; }
        public string? MoTa { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayThayDoi { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<ThanhToan>? ThanhToans { get; set; }
        public virtual List<HoaDon>? HoaDons { get; set; }
	}
}
