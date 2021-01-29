using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bathhouse.EF.Migrations
{
    public partial class AddDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("d1566c88-2ae9-4a16-8956-fa3091bf9eeb"), "711777c3-fdc1-4025-8b13-ae75d1f4f07d", "Admin", "ADMIN" },
                    { new Guid("a9e41cef-fdf4-4c41-a87f-c0fb9dd0cad8"), "f9c745ff-1878-4a96-a51e-4aa359ba65e7", "Director", "DIRECTOR" },
                    { new Guid("13dbef67-f38c-4ca3-9025-a76fb9bb0f6f"), "ee94f595-cc21-4c87-84f9-80c1d84daa4b", "Manager", "MANAGER" },
                    { new Guid("cef7b3d5-a71b-4ee2-913a-7c222927ada3"), "1e182dd3-eee8-4fd5-a878-96563ef5818a", "Employee", "EMPLOYEE" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("13dbef67-f38c-4ca3-9025-a76fb9bb0f6f"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a9e41cef-fdf4-4c41-a87f-c0fb9dd0cad8"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cef7b3d5-a71b-4ee2-913a-7c222927ada3"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d1566c88-2ae9-4a16-8956-fa3091bf9eeb"));
        }
    }
}
