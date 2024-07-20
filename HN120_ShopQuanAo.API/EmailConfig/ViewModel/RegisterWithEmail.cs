using System.ComponentModel.DataAnnotations;

namespace HN120_ShopQuanAo.API.EmailConfig.ViewModel
{
    public class RegisterWithEmail
    {
        [Required(ErrorMessage = "Họ và tên không được bỏ trống")]
        [StringLength(50, ErrorMessage = "Họ và tên không được vượt quá 50 ký tự")]
        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [RegularExpression(@"^(03|05|07|08|09)\d{8,9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }
    }
}
