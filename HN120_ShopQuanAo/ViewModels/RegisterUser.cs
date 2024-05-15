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
		[Required(ErrorMessage = "Username không được bỏ trống")]
		public string? Username { get; set; }

		[EmailAddress]
		public string? Email { get; set; }


		[Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
		public string? PhoneNumber { get; set; }


		[Required(ErrorMessage = "Password không được bỏ trống")]
		public string? Password { get; set; }


		[Required(ErrorMessage = "Confirm password không được bỏ trống")]
		public string? ConfirmPassword { get; set; }
	}
}
