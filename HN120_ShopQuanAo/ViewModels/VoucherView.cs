using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.ViewModels
{
    public  class VoucherView
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "mời bạn nhập Mã")]
        [StringLength(40, ErrorMessage = "Mã không được quá 40 kí tự")]
        public string MaVoucher { get; set; }
        [Required(ErrorMessage = "mời bạn chọn hình thức giảm giá ")]
        public int KieuGiamGia { get; set; }//0 là giảm theo %, 1 là giảm thẳng giá tiền
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        
        public int dieuKienGiam { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
       
        public int giaGiamToiThieu { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        public int giaGiamToiDa { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        public int giaTriGiam { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        public DateTime NgayBatDau { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        public DateTime NgayKetThuc { get; set; }
        [Required(ErrorMessage = "mời bạn nhập dữ liệu")]
        
        public int SoLuong { get; set; }
        
        public int TrangThai { get; set; }
    }
}
