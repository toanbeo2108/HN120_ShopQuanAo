using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Models
{
	public class DeliveryAddress
	{
		[Key] public string DeliveryAddressID { get; set; }
        public string? UserID { get; set; }
        public string? Consignee { get; set; } // Người nhận hàng
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Street { get; set; }
        public int? Status { get; set; }
        public virtual User? User { get; set; }
	}
}
