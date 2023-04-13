using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class correctio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FarmerEmail",
                table: "Products",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FarmerUserName",
                table: "Products",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ProductLocalGovernment",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 3, 27, 22, 53, 38, 703, DateTimeKind.Local).AddTicks(3958), "$2b$10$HIKVCkhwq.rPztr.ABc78ufHuwWLJbBeru0fJT1AJSZ3bh7s10aZC" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FarmerEmail",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FarmerUserName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductLocalGovernment",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 3, 27, 12, 37, 35, 848, DateTimeKind.Local).AddTicks(7611), "$2b$10$siAjI.Zzmkonl4uBDT6HeuGLj4dJKWtvc7n3mLBR1ZEk7gzvdgJDS" });
        }
    }
}
