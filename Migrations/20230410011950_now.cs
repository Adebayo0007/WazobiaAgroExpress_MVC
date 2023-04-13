using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class now : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 10, 2, 19, 50, 455, DateTimeKind.Local).AddTicks(4689), "$2b$10$LPHvoVRYrWElSsDaF/o4weKL4E5v97Ry7QtuCVU2CokzvYMissAg." });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 5, 16, 46, 43, 701, DateTimeKind.Local).AddTicks(2820), "$2b$10$vEnQPMNiqe0CHnTYwC6zlexgSIsBZc7cY5kXYsR2WvxFlmZPiLI/O" });
        }
    }
}
