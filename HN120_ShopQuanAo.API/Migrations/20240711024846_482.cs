using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HN120_ShopQuanAo.API.Migrations
{
    public partial class _482 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06b5c34b-2865-4285-a281-efca3662c841");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7536a5c0-fd90-4318-89d3-355fc225488c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b02b7845-d1b8-4d7d-9f5b-3700b759051a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0156c258-376a-485c-a457-d5d69f30c5a0", "fc60e769-d63c-4023-a67a-6a6b3ed18b77", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "09508c5b-94d1-460c-82b6-d528d805f13b", "7d72eb3c-4ea0-4ee7-8b2f-530ece5988fd", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3504041d-4672-4a91-bdac-82b7dbeac2b8", "5fb6ca91-3676-4b8e-b9fd-998e84d18dce", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0156c258-376a-485c-a457-d5d69f30c5a0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09508c5b-94d1-460c-82b6-d528d805f13b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3504041d-4672-4a91-bdac-82b7dbeac2b8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "06b5c34b-2865-4285-a281-efca3662c841", "c429e427-00cd-4bca-886b-2efb4aed3776", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7536a5c0-fd90-4318-89d3-355fc225488c", "36b9f2db-5b78-44c0-90db-ba08a93ccde7", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b02b7845-d1b8-4d7d-9f5b-3700b759051a", "b7a3abdf-7f94-4574-a676-111fb1b560af", "Admin", "ADMIN" });
        }
    }
}
