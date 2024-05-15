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
		public string Password { get; set; }
	}
}
