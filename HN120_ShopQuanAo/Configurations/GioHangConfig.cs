using HN120_ShopQuanAo.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HN120_ShopQuanAo.Data.Configurations
{
    public class GioHangConfig : IEntityTypeConfiguration<GioHang>
    {
        public void Configure(EntityTypeBuilder<GioHang> builder)
        {
            builder.HasKey(p => p.MaGioHang); // Set khóa chính
            builder.HasOne(p => p.User).WithOne(p => p.GioHang).HasForeignKey<GioHang>(p => p.MaGioHang);
        }
    }
}
