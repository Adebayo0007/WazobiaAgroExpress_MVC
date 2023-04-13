using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class sounding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThumbDown",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThumbUp",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 12, 14, 5, 42, 559, DateTimeKind.Local).AddTicks(5831), "$2b$10$UXAXhN9WlMs7LOnGLNfsmO9/h50h1TkqPvJfiezfVrKuDJRUZRb8K" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbDown",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ThumbUp",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 11, 22, 52, 24, 649, DateTimeKind.Local).AddTicks(6899), "$2b$10$20W47AVPTu879efE.8ee5O3MPaejtaLngjiOK.ThG5ZMQK70QzLlK" });
        }
    }
}
