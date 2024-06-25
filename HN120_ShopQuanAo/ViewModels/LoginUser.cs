using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.ViewModels
{
	public class LoginUser
	{
		[Required(ErrorMessage = "Sdt không được bỏ trống")]
		public string PhoneNumber { get; set; }


		[Required(ErrorMessage = "Password không được bỏ trống")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Mật khẩu phải có ít nhất 1 ký tự viết hoa, 1 ký tự đặc biệt, 1 số và tối thiểu 6 ký tự")]
        public string Password { get; set; }
	}
}
