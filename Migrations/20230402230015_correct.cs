using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class correct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRegistered",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 3, 0, 0, 14, 844, DateTimeKind.Local).AddTicks(9780), "$2b$10$hDYX4cBQ5byQyEe0.M//g.8o1atea6pZ/t7BQHxsadN0OepnoRLAO" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRegistered",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 3, 28, 14, 52, 57, 275, DateTimeKind.Local).AddTicks(557), "$2b$10$NMnwvA5oJC.da5YouuSWGeQzqFTVSmn6QeHpPPCF7jSfcnQ6MgHEq" });
        }
    }
}
