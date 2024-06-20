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
	public class SanPhamConfig : IEntityTypeConfiguration<SanPham>
	{
		public void Configure(EntityTypeBuilder<SanPham> builder)
		{
			builder.HasKey(p => p.MaSp); // Set khóa chính
			builder.HasOne(p => p.ThuongHieu).WithMany(p => p.SanPhams).HasForeignKey(p => p.MaThuongHieu);
			builder.HasOne(p => p.TheLoai).WithMany(p => p.SanPhams).HasForeignKey(p => p.MaTheLoai);
            builder.HasOne(p => p.ChatLieu).WithMany(p => p.SanPhams).HasForeignKey(p => p.MaChatLieu);

        }
    }
}
