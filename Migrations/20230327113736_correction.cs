using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class correction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelivered",
                table: "RequestedProducts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 3, 27, 12, 37, 35, 848, DateTimeKind.Local).AddTicks(7611), "$2b$10$siAjI.Zzmkonl4uBDT6HeuGLj4dJKWtvc7n3mLBR1ZEk7gzvdgJDS" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelivered",
                table: "RequestedProducts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 3, 21, 17, 33, 1, 176, DateTimeKind.Local).AddTicks(3550), "$2b$10$GuGRxeDClvtXTZbJWHzSjO7IEKKysXULZ4DifkseRe/Wgq6MoX8EC" });
        }
    }
}
