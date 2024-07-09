using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HN120_ShopQuanAo.Data.Models
{
    public class GioHang
    {
        [Key]
        public string MaGioHang { get; set; }
        public decimal? TongTien { get; set; }
        public int? MoTa { get; set; }
        public int? TrangThai { get; set; }

        public virtual User? User { get; set; } // Mối quan hệ 1-1 với User
        public virtual List<GioHangChiTiet>? GioHangChiTiets { get; set; } // Thêm thuộc tính này
    }
}
