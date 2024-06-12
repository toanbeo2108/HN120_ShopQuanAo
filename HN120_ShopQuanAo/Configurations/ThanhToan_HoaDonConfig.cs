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
	public class ThanhToan_HoaDonConfig : IEntityTypeConfiguration<ThanhToan_HoaDon>
	{
		public void Configure(EntityTypeBuilder<ThanhToan_HoaDon> builder)
		{
			builder.HasKey(p => p.MaPhuongThuc_HoaDon);
			builder.HasOne(p => p.ThanhToan).WithMany(x => x.ThanhToan_HoaDon).HasForeignKey(x => x.MaPhuongThuc);
			builder.HasOne(p => p.HoaDon).WithMany(x => x.ThanhToan_HoaDons).HasForeignKey(x => x.MaHoaDon);
		}
	}
}
