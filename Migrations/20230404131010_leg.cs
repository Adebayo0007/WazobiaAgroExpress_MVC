using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class leg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "RequestedProducts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 4, 14, 10, 9, 695, DateTimeKind.Local).AddTicks(1273), "$2b$10$BOBMe2rB.1gGHSOiDd/jVOJPJI7BNAE505yTx32zghOSPV4k/hNlG" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "RequestedProducts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 3, 0, 13, 14, 508, DateTimeKind.Local).AddTicks(3764), "$2b$10$EFRIb6wYlf85xGXxyryPWeXaWEWoTP4xFsM8a5wSHP0gvQAKpH2tG" });
        }
    }
}
