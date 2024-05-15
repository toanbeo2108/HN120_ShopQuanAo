using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.ViewModels
{
	public class Response
	{
		public bool IsSuccess { get; set; }
		public int StatusCode { get; set; }
		public string Message { get; set; }
		public string Token { get; set; }
	}
}
