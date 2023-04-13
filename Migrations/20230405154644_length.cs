using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class length : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FarmerName",
                table: "RequestedProducts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FarmerNumber",
                table: "RequestedProducts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 5, 16, 46, 43, 701, DateTimeKind.Local).AddTicks(2820), "$2b$10$vEnQPMNiqe0CHnTYwC6zlexgSIsBZc7cY5kXYsR2WvxFlmZPiLI/O" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FarmerName",
                table: "RequestedProducts");

            migrationBuilder.DropColumn(
                name: "FarmerNumber",
                table: "RequestedProducts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 4, 14, 10, 9, 695, DateTimeKind.Local).AddTicks(1273), "$2b$10$BOBMe2rB.1gGHSOiDd/jVOJPJI7BNAE505yTx32zghOSPV4k/hNlG" });
        }
    }
}
