using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace HN120_ShopQuanAo.Data.Models
{
    public class User : IdentityUser
    {
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
        public int? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string? CardNumber { get; set; }
        public int? Status { get; set; }

        public virtual GioHang? GioHang { get; set; }
        public virtual List<HoaDon>? HoaDons { get; set; }
        public virtual List<DeliveryAddress>? DeliveryAddress { get; set; }
        public virtual List<User_Voucher>? User_Vouchers { get; set; }
    }
}
