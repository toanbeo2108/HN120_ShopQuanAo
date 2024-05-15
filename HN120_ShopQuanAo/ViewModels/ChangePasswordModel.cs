using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.ViewModels
{
	public class ChangePasswordModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Mật khẩu hiện tại")]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "Mật khẩu mới phải có ít nhất {2} và tối đa {1} ký tự.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Mật khẩu mới")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Xác nhận mật khẩu mới")]
		//compare dùng để so sáng giá trị
		[Compare("NewPassword", ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.")]
		public string ConfirmPassword { get; set; }
	}
}
