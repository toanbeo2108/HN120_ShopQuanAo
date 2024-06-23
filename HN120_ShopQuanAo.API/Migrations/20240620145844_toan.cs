using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HN120_ShopQuanAo.API.Migrations
{
    public partial class toan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "346c98a5-40a9-4c09-a4a0-2522ac7d0464");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "479285a7-5a58-4782-8ca9-233f63ee1e5a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8175ab3a-2e02-49d2-93e7-117747733257");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "030a3804-122b-42b9-900f-809f49be059f", "8eea4e57-03c2-47e3-9dc2-f40bb59edfe0", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4c6ecd1c-ca3a-4596-9d06-36312030ed0a", "5dc0c606-20cb-46e3-97c7-712e8c63c7c6", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8b327fad-c3d2-4984-9962-14527d22f86d", "834943f2-391f-4dfe-a77d-3e231a1c3796", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "030a3804-122b-42b9-900f-809f49be059f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c6ecd1c-ca3a-4596-9d06-36312030ed0a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b327fad-c3d2-4984-9962-14527d22f86d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "346c98a5-40a9-4c09-a4a0-2522ac7d0464", "8a4d40ad-63ec-4c1d-ad44-be1eefe5a29d", "Employee", "EMPLOYEE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "479285a7-5a58-4782-8ca9-233f63ee1e5a", "32debd2a-9133-4e91-880e-f791100b3c11", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8175ab3a-2e02-49d2-93e7-117747733257", "68443cbf-f1cf-49f6-ac2b-5a3be7918446", "Admin", "ADMIN" });
        }
    }
}
