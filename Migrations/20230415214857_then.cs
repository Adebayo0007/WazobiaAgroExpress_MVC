using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class then : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Haspaid",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 15, 22, 48, 57, 86, DateTimeKind.Local).AddTicks(3632), "$2b$10$V4W9NoI68yNI0LJgUnoDp.l.YbtF6jNffj2G8wMfpytRyfPiWBFnS" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Haspaid",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 12, 14, 5, 42, 559, DateTimeKind.Local).AddTicks(5831), "$2b$10$UXAXhN9WlMs7LOnGLNfsmO9/h50h1TkqPvJfiezfVrKuDJRUZRb8K" });
        }
    }
}
