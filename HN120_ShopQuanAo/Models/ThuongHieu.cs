using System;
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
        public string? TenThuongHieu { get; set; }
        public string? MoTa { get; set; }
        public int? TrangThai { get; set; }
        public virtual List<SanPham>? SanPhams { get; set; }
	}
}
