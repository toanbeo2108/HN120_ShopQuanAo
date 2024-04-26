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
	public class TheLoaiConfig : IEntityTypeConfiguration<TheLoai>
	{
		public void Configure(EntityTypeBuilder<TheLoai> builder)
		{
			builder.HasKey(p => p.MaTheLoai);
		}
	}
}
