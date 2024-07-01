using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.ViewModels
{
    public class DeliveryAddressModel
    {
        public string? UserID { get; set; }


        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Họ và tên không được vượt quá 50 ký tự")]
        public string? Consignee { get; set; }


        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^(03|05|07|08|09)\d{8,9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }


        [Required(ErrorMessage = "Tỉnh/Thành phố là bắt buộc.")]
        public string? City { get; set; }


        [Required(ErrorMessage = "Quận/Huyện là bắt buộc.")]
        public string? District { get; set; }


        [Required(ErrorMessage = "Phường/Xã là bắt buộc.")]
        public string? Ward { get; set; }


        [Required(ErrorMessage = "Địa chỉ cụ thể là bắt buộc.")]
        public string? Street { get; set; }
        public int? Status { get; set; }
    }
}
