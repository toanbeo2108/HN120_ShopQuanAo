using HN120_ShopQuanAo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HN120_ShopQuanAo.Data.Configurations
{
	internal class UserVoucherConfig : IEntityTypeConfiguration<User_Voucher>
	{
		public void Configure(EntityTypeBuilder<User_Voucher> builder)
		{
			builder.HasKey(p => p.MaVoucher);
			builder.HasOne(p => p.User).WithMany(p => p.User_Vouchers).HasForeignKey(p => p.UserID);
			builder.HasOne(p => p.Voucher).WithMany(p => p.User_Vouchers).HasForeignKey(p => p.MaVoucher);
		}
	}
}
