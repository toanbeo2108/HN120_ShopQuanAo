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
		[Key]
		public string DeliveryAddressID { get; set; }
        public string? UserID { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AddressLine { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public virtual User? User { get; set; }
	}
}
