using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.ViewModels
{
    public class UserViewModel : IdentityUser
    {
        public string? Avatar { get; set; } // Link ảnh đại diện
        public string? FullName { get; set; }
        public int? Gender { get; set; }// Giới tính
        public DateTime? Birthday { get; set; }
        public string? CardNumber { get; set; }
        public int? Status { get; set; } // trạng thái : 0 == Ko còn hoạt động, ẩn nick ,....
    }
}
