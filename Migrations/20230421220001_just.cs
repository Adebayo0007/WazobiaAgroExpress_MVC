using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class just : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotDelivered",
                table: "RequestedProducts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 21, 23, 0, 0, 700, DateTimeKind.Local).AddTicks(2592), "$2b$10$Jf8x/F3cnFnk2GLJat/Pb.lLncAsViAm4qVTj5flSWNUyDWtEy9Am" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotDelivered",
                table: "RequestedProducts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 16, 0, 20, 16, 585, DateTimeKind.Local).AddTicks(9957), "$2b$10$2FACaT03XVHZrB5bUjapougV1jwdzeo/UkzqBAkX/GDqxCgD/J1XS" });
        }
    }
}
