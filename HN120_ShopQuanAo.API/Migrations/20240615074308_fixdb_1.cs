using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HN120_ShopQuanAo.API.Migrations
{
    public partial class fixdb_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7eefb2d4-87bd-49fa-9780-ed2847642b50");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc642f12-74d8-4984-8526-03d24b5ce910");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6fb98c5-0523-4e3d-8078-a961996b1af2");

            migrationBuilder.AddColumn<string>(
                name: "UrlAnhSpct",
                table: "ChiTietSp",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "28d9da66-bdac-4bb3-9aa4-3e34a5410d18", "8e9ec98a-e446-445f-bfcc-9a6d4d1bcf3a", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6053ea0d-a054-424c-b062-48904c8de5a5", "86c98ad6-05da-45fe-bfc4-4912fc5eff95", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a99210c9-d1e4-488d-895b-c04a8c03b022", "77a5df7e-d58e-4bbf-837c-7c9a5ee6a697", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28d9da66-bdac-4bb3-9aa4-3e34a5410d18");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6053ea0d-a054-424c-b062-48904c8de5a5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a99210c9-d1e4-488d-895b-c04a8c03b022");

            migrationBuilder.DropColumn(
                name: "UrlAnhSpct",
                table: "ChiTietSp");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7eefb2d4-87bd-49fa-9780-ed2847642b50", "91874859-4808-4d81-ac61-795aa86795ca", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bc642f12-74d8-4984-8526-03d24b5ce910", "7519cfc7-cf6f-4ffd-941b-815cb2866fd2", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f6fb98c5-0523-4e3d-8078-a961996b1af2", "58b0beca-727f-45b5-bd84-350826c77fa4", "Admin", "ADMIN" });
        }
    }
}
