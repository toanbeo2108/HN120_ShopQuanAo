using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.ViewModels
{
	public class RegisterUser
	{
        [Required(ErrorMessage = "Họ và tên không được bỏ trống")]
        [StringLength(50, ErrorMessage = "Họ và tên không được vượt quá 50 ký tự")]
        public string? FullName { get; set; }

		[EmailAddress]
		public string? Email { get; set; }


		[Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        [RegularExpression(@"^(03|05|07|08|09)\d{8,9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }


		[Required(ErrorMessage = "Password không được bỏ trống")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Mật khẩu phải có ít nhất 1 ký tự viết hoa, 1 ký tự đặc biệt, 1 số và tối thiểu 6 ký tự")]
        public string? Password { get; set; }


		[Required(ErrorMessage = "Confirm password không được bỏ trống")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp!")]
        public string? ConfirmPassword { get; set; }
	}
}
