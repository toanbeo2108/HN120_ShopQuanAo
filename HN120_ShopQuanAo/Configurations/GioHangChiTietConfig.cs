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
	public class GioHangChiTietConfig : IEntityTypeConfiguration<GioHangChiTiet>
	{
		public void Configure(EntityTypeBuilder<GioHangChiTiet> builder)
		{
			builder.HasKey(p => p.MaGioHangChiTiet); // Set khóa chính
			builder.HasOne(p => p.GioHang).WithMany(p => p.GioHangChiTiets).HasForeignKey(p => p.SKU);
			builder.HasOne(p => p.ChiTietSps).WithMany(p => p.GioHangChiTiet).HasForeignKey(p => p.MaGioHang);
		}
	}
}
