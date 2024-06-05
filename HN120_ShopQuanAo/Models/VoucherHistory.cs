using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class VoucherHistory
	{
		[Key] public int Id { get; set; }
        public string? MaVoucher { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayThayDoi { get; set; }
        public decimal? GiaGiamToiThieu { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public int? KieuGiamGia { get; set; }
        public decimal? GiaTriGiam { get; set; }
        public int? TrangThai { get; set; }

        public virtual Voucher? Voucher { get; set; }
	}
}
