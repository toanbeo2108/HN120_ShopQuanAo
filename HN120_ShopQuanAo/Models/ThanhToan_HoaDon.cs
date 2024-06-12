using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual ThanhToan? ThanhToan { get; set; }
        public virtual HoaDon? HoaDon { get; set; }
	}
}
