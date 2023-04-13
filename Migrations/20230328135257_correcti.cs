using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class correcti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductState",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 3, 28, 14, 52, 57, 275, DateTimeKind.Local).AddTicks(557), "$2b$10$NMnwvA5oJC.da5YouuSWGeQzqFTVSmn6QeHpPPCF7jSfcnQ6MgHEq" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductState",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 3, 27, 22, 53, 38, 703, DateTimeKind.Local).AddTicks(3958), "$2b$10$HIKVCkhwq.rPztr.ABc78ufHuwWLJbBeru0fJT1AJSZ3bh7s10aZC" });
        }
    }
}
