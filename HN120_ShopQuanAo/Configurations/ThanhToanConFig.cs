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
	public class ThanhToanConFig : IEntityTypeConfiguration<ThanhToan>
	{
		public void Configure(EntityTypeBuilder<ThanhToan> builder)
		{
			builder.HasKey(p => p.MaPhuongThuc); // Set khóa chính
			//builder.HasOne(x => x.ThanhToan_HoaDon).WithMany(x => x.ThanhToanss).HasForeignKey(x => x.MaPhuongThuc);
		}
	}
}
