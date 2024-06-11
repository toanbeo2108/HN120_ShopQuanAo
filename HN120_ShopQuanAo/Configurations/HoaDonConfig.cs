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
	public class HoaDonConfig : IEntityTypeConfiguration<HoaDon>
	{
		public void Configure(EntityTypeBuilder<HoaDon> builder)
		{
			builder.HasKey(p => p.MaHoaDon); // Set khóa chính
			builder.HasOne(p => p.User).WithMany(p => p.HoaDons).HasForeignKey(p => p.UserID);
			builder.HasOne(p => p.Voucher).WithMany(p => p.HoaDons).HasForeignKey(p => p.MaVoucher);
			builder.HasMany(p => p.HoaDon_History).WithOne(p => p.HoaDon).HasForeignKey(p => p.MaHoaDon);
			//builder.HasOne(x => x.ThanhToan_HoaDon).WithMany(x => x.HoaDonss).HasForeignKey(x => x.MaHoaDon);
		}
	}
}
