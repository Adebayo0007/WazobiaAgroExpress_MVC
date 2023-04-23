using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class juse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Due",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Due", "Password" },
                values: new object[] { new DateTime(2023, 4, 22, 2, 18, 38, 161, DateTimeKind.Local).AddTicks(1397), true, "$2b$10$YYPwR0ukmU7eMnab06I55Ouxwe8LcCsQzD4hKzhhzJIrdDx0pB87a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Due",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 22, 1, 21, 13, 864, DateTimeKind.Local).AddTicks(8525), "$2b$10$R26on4jNXUUIMs/EsK24feT.BrclOOLGfPILibAEAMUGrql11Husu" });
        }
    }
}
