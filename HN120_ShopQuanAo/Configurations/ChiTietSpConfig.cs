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
	public class ChiTietSpConfig : IEntityTypeConfiguration<ChiTietSp>
	{
		public void Configure(EntityTypeBuilder<ChiTietSp> builder)
		{
			builder.HasKey(p => p.SKU); // Set khóa chính
			builder.HasOne(p => p.SanPham).WithMany(p => p.ChiTietSps).HasForeignKey(p => p.MaSp);
			builder.HasOne(p => p.TheLoai).WithMany(p => p.ChiTietSps).HasForeignKey(p => p.MaTheLoai);
			builder.HasOne(p => p.Size).WithMany(p => p.ChiTietSps).HasForeignKey(p => p.MaSize);
			builder.HasOne(p => p.MauSac).WithMany(p => p.ChiTietSps).HasForeignKey(p => p.MaMau);
			builder.HasOne(p => p.KhuyenMai).WithMany(p => p.ChiTietSps).HasForeignKey(p => p.MaKhuyenMai);
			builder.HasOne(p => p.ChatLieu).WithMany(p => p.ChiTietSps).HasForeignKey(p => p.MaChatLieu);
		}
	}
}
