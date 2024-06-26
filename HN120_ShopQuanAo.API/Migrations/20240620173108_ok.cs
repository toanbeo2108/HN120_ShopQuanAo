using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HN120_ShopQuanAo.API.Migrations
{
    public partial class ok : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnhSanPham",
                columns: table => new
                {
                    MaAnh = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSP_MaSPCT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnhSanPham", x => x.MaAnh);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatLieu",
                columns: table => new
                {
                    MaChatLieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenChatLieu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatLieu", x => x.MaChatLieu);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhanTramGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMai", x => x.MaKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "MauSac",
                columns: table => new
                {
                    MaMau = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenMau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauSac", x => x.MaMau);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    MaSize = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenSize = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.MaSize);
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    MaPhuongThuc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenPhuongThuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayThayDoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan", x => x.MaPhuongThuc);
                });

            migrationBuilder.CreateTable(
                name: "TheLoai",
                columns: table => new
                {
                    MaTheLoai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenTheLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheLoai", x => x.MaTheLoai);
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieu",
                columns: table => new
                {
                    MaThuongHieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuongHieu", x => x.MaThuongHieu);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    MaVoucher = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DieuKienGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiaGiamToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiaGiamToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KieuGiamGia = table.Column<int>(type: "int", nullable: true),
                    GiaTriGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.MaVoucher);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAddress",
                columns: table => new
                {
                    DeliveryAddressID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddress", x => x.DeliveryAddressID);
                    table.ForeignKey(
                        name: "FK_DeliveryAddress_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    MaGioHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MoTa = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK_GioHang_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    MaSp = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaThuongHieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaTheLoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaChatLieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlAvatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenSP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mota = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TongSoLuong = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    ThuongHieuMaThuongHieu = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TheLoaiMaTheLoai = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChatLieuMaChatLieu = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.MaSp);
                    table.ForeignKey(
                        name: "FK_SanPham_ChatLieu_ChatLieuMaChatLieu",
                        column: x => x.ChatLieuMaChatLieu,
                        principalTable: "ChatLieu",
                        principalColumn: "MaChatLieu");
                    table.ForeignKey(
                        name: "FK_SanPham_TheLoai_TheLoaiMaTheLoai",
                        column: x => x.TheLoaiMaTheLoai,
                        principalTable: "TheLoai",
                        principalColumn: "MaTheLoai");
                    table.ForeignKey(
                        name: "FK_SanPham_ThuongHieu_ThuongHieuMaThuongHieu",
                        column: x => x.ThuongHieuMaThuongHieu,
                        principalTable: "ThuongHieu",
                        principalColumn: "MaThuongHieu");
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    MaHoaDon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaVoucher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTaoDon = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhiShip = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TongGiaTriHangHoa = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhuongThucThanhToan = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    VoucherMaVoucher = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon", x => x.MaHoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDon_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HoaDon_Voucher_VoucherMaVoucher",
                        column: x => x.VoucherMaVoucher,
                        principalTable: "Voucher",
                        principalColumn: "MaVoucher");
                });

            migrationBuilder.CreateTable(
                name: "User_Voucher",
                columns: table => new
                {
                    UserVoucherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaVoucher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    VoucherMaVoucher = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Voucher", x => x.UserVoucherID);
                    table.ForeignKey(
                        name: "FK_User_Voucher_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_Voucher_Voucher_VoucherMaVoucher",
                        column: x => x.VoucherMaVoucher,
                        principalTable: "Voucher",
                        principalColumn: "MaVoucher");
                });

            migrationBuilder.CreateTable(
                name: "VoucherHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaVoucher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayThayDoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GiaGiamToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KieuGiamGia = table.Column<int>(type: "int", nullable: true),
                    GiaTriGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    VoucherMaVoucher = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherHistory_Voucher_VoucherMaVoucher",
                        column: x => x.VoucherMaVoucher,
                        principalTable: "Voucher",
                        principalColumn: "MaVoucher");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietSp",
                columns: table => new
                {
                    SKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaMau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlAnhSpct = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SoLuongTon = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    SanPhamMaSp = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SizeMaSize = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MauSacMaMau = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    KhuyenMaiMaKhuyenMai = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietSp", x => x.SKU);
                    table.ForeignKey(
                        name: "FK_ChiTietSp_KhuyenMai_KhuyenMaiMaKhuyenMai",
                        column: x => x.KhuyenMaiMaKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "MaKhuyenMai");
                    table.ForeignKey(
                        name: "FK_ChiTietSp_MauSac_MauSacMaMau",
                        column: x => x.MauSacMaMau,
                        principalTable: "MauSac",
                        principalColumn: "MaMau");
                    table.ForeignKey(
                        name: "FK_ChiTietSp_SanPham_SanPhamMaSp",
                        column: x => x.SanPhamMaSp,
                        principalTable: "SanPham",
                        principalColumn: "MaSp");
                    table.ForeignKey(
                        name: "FK_ChiTietSp_Size_SizeMaSize",
                        column: x => x.SizeMaSize,
                        principalTable: "Size",
                        principalColumn: "MaSize");
                });

            migrationBuilder.CreateTable(
                name: "HoaDon_History",
                columns: table => new
                {
                    LichSuHoaDonID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaHoaDon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTaoDon = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayThayDoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TongGiaTri = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HinhThucThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    HoaDonMaHoaDon = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon_History", x => x.LichSuHoaDonID);
                    table.ForeignKey(
                        name: "FK_HoaDon_History_HoaDon_HoaDonMaHoaDon",
                        column: x => x.HoaDonMaHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "MaHoaDon");
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan_HoaDon",
                columns: table => new
                {
                    MaPhuongThuc_HoaDon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaHoaDon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaPhuongThuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayThayDoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    ThanhToanMaPhuongThuc = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HoaDonMaHoaDon = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan_HoaDon", x => x.MaPhuongThuc_HoaDon);
                    table.ForeignKey(
                        name: "FK_ThanhToan_HoaDon_HoaDon_HoaDonMaHoaDon",
                        column: x => x.HoaDonMaHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "MaHoaDon");
                    table.ForeignKey(
                        name: "FK_ThanhToan_HoaDon_ThanhToan_ThanhToanMaPhuongThuc",
                        column: x => x.ThanhToanMaPhuongThuc,
                        principalTable: "ThanhToan",
                        principalColumn: "MaPhuongThuc");
                });

            migrationBuilder.CreateTable(
                name: "GioHangChiTiet",
                columns: table => new
                {
                    MaGioHangChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaGioHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenSp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonGia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoLuong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    GioHangMaGioHang = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChiTietSpsSKU = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangChiTiet", x => x.MaGioHangChiTiet);
                    table.ForeignKey(
                        name: "FK_GioHangChiTiet_ChiTietSp_ChiTietSpsSKU",
                        column: x => x.ChiTietSpsSKU,
                        principalTable: "ChiTietSp",
                        principalColumn: "SKU");
                    table.ForeignKey(
                        name: "FK_GioHangChiTiet_GioHang_GioHangMaGioHang",
                        column: x => x.GioHangMaGioHang,
                        principalTable: "GioHang",
                        principalColumn: "MaGioHang");
                });

            migrationBuilder.CreateTable(
                name: "HoaDonChiTiet",
                columns: table => new
                {
                    MaHoaDonChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaHoaDon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenSp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SoLuongMua = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    HoaDonMaHoaDon = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChiTietSpsSKU = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonChiTiet", x => x.MaHoaDonChiTiet);
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiet_ChiTietSp_ChiTietSpsSKU",
                        column: x => x.ChiTietSpsSKU,
                        principalTable: "ChiTietSp",
                        principalColumn: "SKU");
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiet_HoaDon_HoaDonMaHoaDon",
                        column: x => x.HoaDonMaHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "MaHoaDon");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "552b2ba4-adc4-45a6-b696-ea41ab55c5c8", "003230d8-5c84-4807-aa31-5559d64096d0", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6292639f-0d0c-4059-b906-38a0c0b1a10e", "c3fc5d9e-b913-40e5-bd94-707eb81ffa24", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ba61c5ce-de49-4d57-9c6a-527f54e43b60", "d98094b3-d1a3-45c5-b060-6d1134330208", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSp_KhuyenMaiMaKhuyenMai",
                table: "ChiTietSp",
                column: "KhuyenMaiMaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSp_MauSacMaMau",
                table: "ChiTietSp",
                column: "MauSacMaMau");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSp_SanPhamMaSp",
                table: "ChiTietSp",
                column: "SanPhamMaSp");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSp_SizeMaSize",
                table: "ChiTietSp",
                column: "SizeMaSize");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_UserID",
                table: "DeliveryAddress",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_UserID",
                table: "GioHang",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTiet_ChiTietSpsSKU",
                table: "GioHangChiTiet",
                column: "ChiTietSpsSKU");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTiet_GioHangMaGioHang",
                table: "GioHangChiTiet",
                column: "GioHangMaGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_UserID",
                table: "HoaDon",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_VoucherMaVoucher",
                table: "HoaDon",
                column: "VoucherMaVoucher");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_History_HoaDonMaHoaDon",
                table: "HoaDon_History",
                column: "HoaDonMaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiet_ChiTietSpsSKU",
                table: "HoaDonChiTiet",
                column: "ChiTietSpsSKU");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiet_HoaDonMaHoaDon",
                table: "HoaDonChiTiet",
                column: "HoaDonMaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_ChatLieuMaChatLieu",
                table: "SanPham",
                column: "ChatLieuMaChatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_TheLoaiMaTheLoai",
                table: "SanPham",
                column: "TheLoaiMaTheLoai");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_ThuongHieuMaThuongHieu",
                table: "SanPham",
                column: "ThuongHieuMaThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_HoaDon_HoaDonMaHoaDon",
                table: "ThanhToan_HoaDon",
                column: "HoaDonMaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_HoaDon_ThanhToanMaPhuongThuc",
                table: "ThanhToan_HoaDon",
                column: "ThanhToanMaPhuongThuc");

            migrationBuilder.CreateIndex(
                name: "IX_User_Voucher_UserID",
                table: "User_Voucher",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Voucher_VoucherMaVoucher",
                table: "User_Voucher",
                column: "VoucherMaVoucher");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherHistory_VoucherMaVoucher",
                table: "VoucherHistory",
                column: "VoucherMaVoucher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnhSanPham");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DeliveryAddress");

            migrationBuilder.DropTable(
                name: "GioHangChiTiet");

            migrationBuilder.DropTable(
                name: "HoaDon_History");

            migrationBuilder.DropTable(
                name: "HoaDonChiTiet");

            migrationBuilder.DropTable(
                name: "ThanhToan_HoaDon");

            migrationBuilder.DropTable(
                name: "User_Voucher");

            migrationBuilder.DropTable(
                name: "VoucherHistory");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "ChiTietSp");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "MauSac");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "ChatLieu");

            migrationBuilder.DropTable(
                name: "TheLoai");

            migrationBuilder.DropTable(
                name: "ThuongHieu");
        }
    }
}
