using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HN120_ShopQuanAo.Data.Models;
using System;

namespace HN120_ShopQuanAo.API.Data
{
	public class AppDbContext : IdentityDbContext<IdentityUser>
	{
		public AppDbContext()
		{
		}
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
		public virtual DbSet<User> User { get; set; }
		public virtual DbSet<ChiTietSp> ChiTietSp { get; set; }
		public virtual DbSet<GioHang> GioHang { get; set; }
		public virtual DbSet<GioHangChiTiet> GioHangChiTiet { get; set; }
		public virtual DbSet<HoaDon> HoaDon { get; set; }
		public virtual DbSet<HoaDonChiTiet> HoaDonChiTiet { get; set; }
		public virtual DbSet<MauSac> MauSac { get; set; }
		public virtual DbSet<SanPham> SanPham { get; set; }
		public virtual DbSet<Size> Size { get; set; }
		public virtual DbSet<TheLoai> TheLoai { get; set; }
		public virtual DbSet<Voucher> Voucher { get; set; }
		public virtual DbSet<AnhSanPham> AnhSanPham { get; set; }
		public virtual DbSet<ChatLieu> ChatLieu { get; set; }
		public virtual DbSet<DeliveryAddress> DeliveryAddress { get; set; }
		public virtual DbSet<KhuyenMai> KhuyenMai { get; set; }
		public virtual DbSet<ThanhToan> ThanhToan { get; set; }
		public virtual DbSet<ThanhToan_HoaDon> ThanhToan_HoaDon { get; set; }
		public virtual DbSet<ThuongHieu> ThuongHieu { get; set; }
		public virtual DbSet<User_Voucher> User_Voucher { get; set; }
		public virtual DbSet<VoucherHistory> VoucherHistory { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			CreateRoles(builder);
		}
		private void CreateRoles(ModelBuilder builder)
		{
			builder.Entity<IdentityRole>().HasData(
					new IdentityRole() { Name = "Admin", NormalizedName = "ADMIN" },
					new IdentityRole() { Name = "User", NormalizedName = "USER" },
					new IdentityRole() { Name = "Employee", NormalizedName = "EMPLOYEE" }
				);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Data Source=LAPTOP-DAV1LO0Q\\SQLEXPRESS;Initial Catalog=ShopQuanAoOnline;Integrated Security=True;");
			}
		}

	}
}
