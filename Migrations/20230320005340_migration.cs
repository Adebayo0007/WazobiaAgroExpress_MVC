using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 3, 20, 1, 53, 39, 480, DateTimeKind.Local).AddTicks(5501), "$2b$10$O/tLV5YezPfzF9VnRbSL7OhBCpsiD3M0HVKS9Dz.XDk1CXHh6x556" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 3, 19, 23, 37, 13, 674, DateTimeKind.Local).AddTicks(8600), "$2b$10$s.7ojcnKDxRkDNaGQNB5veEUVNm0XyTpG0lg4r5FgB4FvTPIeMFgy" });
        }
    }
}
