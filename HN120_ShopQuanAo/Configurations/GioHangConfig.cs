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
	public class GioHangConfig : IEntityTypeConfiguration<GioHang>
	{
		public void Configure(EntityTypeBuilder<GioHang> builder)
		{
			builder.HasKey(p => p.MaGioHang); // Set khóa chính
			builder.HasOne(p => p.User).WithMany(p => p.GioHangs).HasForeignKey(p => p.UserID);
		}
	}
}
