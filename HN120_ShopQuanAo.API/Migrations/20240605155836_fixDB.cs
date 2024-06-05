using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HN120_ShopQuanAo.API.Migrations
{
    public partial class fixDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d30179b-b5ae-4057-bbdf-bcb46943dc31");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28d4376e-4e9c-49d5-8c83-e1453b2c818a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e2a6036-066f-450b-867a-31070808f9e6");

            migrationBuilder.DropColumn(
                name: "ChietKhau",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "TenVoucher",
                table: "Voucher");

            migrationBuilder.RenameColumn(
                name: "GiamTien",
                table: "Voucher",
                newName: "GiaTriGiam");

            migrationBuilder.RenameColumn(
                name: "DieuKien",
                table: "Voucher",
                newName: "GiaGiamToiThieu");

            migrationBuilder.AddColumn<decimal>(
                name: "DieuKienGiam",
                table: "Voucher",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GiaGiamToiDa",
                table: "Voucher",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KieuGiamGia",
                table: "Voucher",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayBatDau",
                table: "Voucher",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKetThuc",
                table: "Voucher",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoLuong",
                table: "Voucher",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaThuongHieu",
                table: "SanPham",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThuongHieuMaThuongHieu",
                table: "SanPham",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlAvatar",
                table: "SanPham",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThanhToan_HoaDonMaPhuongThuc_HoaDon",
                table: "HoaDon",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChatLieuMaChatLieu",
                table: "ChiTietSp",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KhuyenMaiMaKhuyenMai",
                table: "ChiTietSp",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaChatLieu",
                table: "ChiTietSp",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaKhuyenMai",
                table: "ChiTietSp",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatLieu",
                columns: table => new
                {
                    MaChatLieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenChatLieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatLieu", x => x.MaChatLieu);
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
                name: "KhuyenMai",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhanTramGiam = table.Column<float>(type: "real", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMai", x => x.MaKhuyenMai);
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
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan_HoaDon", x => x.MaPhuongThuc_HoaDon);
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieu",
                columns: table => new
                {
                    MaThuongHieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuongHieu", x => x.MaThuongHieu);
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
                name: "ThanhToan",
                columns: table => new
                {
                    MaPhuongThuc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenPhuongThuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayThayDoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    ThanhToan_HoaDonMaPhuongThuc_HoaDon = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan", x => x.MaPhuongThuc);
                    table.ForeignKey(
                        name: "FK_ThanhToan_ThanhToan_HoaDon_ThanhToan_HoaDonMaPhuongThuc_HoaDon",
                        column: x => x.ThanhToan_HoaDonMaPhuongThuc_HoaDon,
                        principalTable: "ThanhToan_HoaDon",
                        principalColumn: "MaPhuongThuc_HoaDon");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5f5aa680-663d-496b-b7c8-4e7275212be8", "4da265ad-a15d-46ce-baee-3976fc934a63", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "86b97d49-0141-42b4-ab2e-4c68e6a48fab", "b2c66434-2522-4cf4-a723-71e3a1c1690e", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9f71fb15-3424-4659-818d-fed209b38ca4", "a592b82f-ca1b-44d4-ab0b-a0e496573032", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_ThuongHieuMaThuongHieu",
                table: "SanPham",
                column: "ThuongHieuMaThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_ThanhToan_HoaDonMaPhuongThuc_HoaDon",
                table: "HoaDon",
                column: "ThanhToan_HoaDonMaPhuongThuc_HoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSp_ChatLieuMaChatLieu",
                table: "ChiTietSp",
                column: "ChatLieuMaChatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSp_KhuyenMaiMaKhuyenMai",
                table: "ChiTietSp",
                column: "KhuyenMaiMaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_UserID",
                table: "DeliveryAddress",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_History_HoaDonMaHoaDon",
                table: "HoaDon_History",
                column: "HoaDonMaHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_ThanhToan_HoaDonMaPhuongThuc_HoaDon",
                table: "ThanhToan",
                column: "ThanhToan_HoaDonMaPhuongThuc_HoaDon");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietSp_ChatLieu_ChatLieuMaChatLieu",
                table: "ChiTietSp",
                column: "ChatLieuMaChatLieu",
                principalTable: "ChatLieu",
                principalColumn: "MaChatLieu");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietSp_KhuyenMai_KhuyenMaiMaKhuyenMai",
                table: "ChiTietSp",
                column: "KhuyenMaiMaKhuyenMai",
                principalTable: "KhuyenMai",
                principalColumn: "MaKhuyenMai");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_ThanhToan_HoaDon_ThanhToan_HoaDonMaPhuongThuc_HoaDon",
                table: "HoaDon",
                column: "ThanhToan_HoaDonMaPhuongThuc_HoaDon",
                principalTable: "ThanhToan_HoaDon",
                principalColumn: "MaPhuongThuc_HoaDon");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_ThuongHieu_ThuongHieuMaThuongHieu",
                table: "SanPham",
                column: "ThuongHieuMaThuongHieu",
                principalTable: "ThuongHieu",
                principalColumn: "MaThuongHieu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietSp_ChatLieu_ChatLieuMaChatLieu",
                table: "ChiTietSp");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietSp_KhuyenMai_KhuyenMaiMaKhuyenMai",
                table: "ChiTietSp");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_ThanhToan_HoaDon_ThanhToan_HoaDonMaPhuongThuc_HoaDon",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_ThuongHieu_ThuongHieuMaThuongHieu",
                table: "SanPham");

            migrationBuilder.DropTable(
                name: "ChatLieu");

            migrationBuilder.DropTable(
                name: "DeliveryAddress");

            migrationBuilder.DropTable(
                name: "HoaDon_History");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "ThuongHieu");

            migrationBuilder.DropTable(
                name: "User_Voucher");

            migrationBuilder.DropTable(
                name: "VoucherHistory");

            migrationBuilder.DropTable(
                name: "ThanhToan_HoaDon");

            migrationBuilder.DropIndex(
                name: "IX_SanPham_ThuongHieuMaThuongHieu",
                table: "SanPham");

            migrationBuilder.DropIndex(
                name: "IX_HoaDon_ThanhToan_HoaDonMaPhuongThuc_HoaDon",
                table: "HoaDon");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietSp_ChatLieuMaChatLieu",
                table: "ChiTietSp");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietSp_KhuyenMaiMaKhuyenMai",
                table: "ChiTietSp");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f5aa680-663d-496b-b7c8-4e7275212be8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86b97d49-0141-42b4-ab2e-4c68e6a48fab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f71fb15-3424-4659-818d-fed209b38ca4");

            migrationBuilder.DropColumn(
                name: "DieuKienGiam",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "GiaGiamToiDa",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "KieuGiamGia",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "NgayBatDau",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "NgayKetThuc",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "SoLuong",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "MaThuongHieu",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "ThuongHieuMaThuongHieu",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "UrlAvatar",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "ThanhToan_HoaDonMaPhuongThuc_HoaDon",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "ChatLieuMaChatLieu",
                table: "ChiTietSp");

            migrationBuilder.DropColumn(
                name: "KhuyenMaiMaKhuyenMai",
                table: "ChiTietSp");

            migrationBuilder.DropColumn(
                name: "MaChatLieu",
                table: "ChiTietSp");

            migrationBuilder.DropColumn(
                name: "MaKhuyenMai",
                table: "ChiTietSp");

            migrationBuilder.RenameColumn(
                name: "GiaTriGiam",
                table: "Voucher",
                newName: "GiamTien");

            migrationBuilder.RenameColumn(
                name: "GiaGiamToiThieu",
                table: "Voucher",
                newName: "DieuKien");

            migrationBuilder.AddColumn<float>(
                name: "ChietKhau",
                table: "Voucher",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenVoucher",
                table: "Voucher",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1d30179b-b5ae-4057-bbdf-bcb46943dc31", "d6e34489-dec5-4d74-bd3e-eb82949c0d82", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "28d4376e-4e9c-49d5-8c83-e1453b2c818a", "8d6a522b-b58f-41ec-9d24-05624d9c28fc", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2e2a6036-066f-450b-867a-31070808f9e6", "92c413e0-4236-447f-a610-37b4210ef358", "Employee", "EMPLOYEE" });
        }
    }
}
