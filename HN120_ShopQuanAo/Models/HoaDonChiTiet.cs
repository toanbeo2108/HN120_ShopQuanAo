using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class HoaDonChiTiet
	{
        [Key]
        public string MaHoaDonChiTiet { get; set; }
        public string? SKU { get; set; }
        public string? MaHoaDon { get; set; }
		public string? TenSp { get; set; }
        public decimal? DonGia { get; set; }
        public int? SoLuongMua { get; set; }
        public int?  TrangThai { get; set; }

        public virtual HoaDon? HoaDon { get; set; }
        public virtual ChiTietSp? ChiTietSps { get; set; }
	}
}
