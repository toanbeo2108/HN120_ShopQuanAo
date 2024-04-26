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
	public class MauSacConfig : IEntityTypeConfiguration<MauSac>
	{
		public void Configure(EntityTypeBuilder<MauSac> builder)
		{
			builder.HasKey(p => p.MaMau); // Set khóa chính
		}
	}
}
