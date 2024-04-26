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
	public class HoaDonChiTietConfig : IEntityTypeConfiguration<HoaDonChiTiet>
	{
		public void Configure(EntityTypeBuilder<HoaDonChiTiet> builder)
		{
			builder.HasKey(p => p.MaHoaDonChiTiet); // Set khóa chính
			builder.HasOne(p => p.HoaDon).WithMany(p => p.HoaDonChiTiets).HasForeignKey(p => p.MaHoaDon);
			builder.HasOne(p => p.ChiTietSps).WithMany(p => p.HoaDonChiTiet).HasForeignKey(p => p.SKU);
		}
	}
}
