using HN120_ShopQuanAo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.ViewModels
{
    public class VoucherViewModel
    {
        public List<Voucher> Vouchers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
