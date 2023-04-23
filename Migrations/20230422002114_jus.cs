using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class jus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FarmerEmail",
                table: "RequestedProducts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 22, 1, 21, 13, 864, DateTimeKind.Local).AddTicks(8525), "$2b$10$R26on4jNXUUIMs/EsK24feT.BrclOOLGfPILibAEAMUGrql11Husu" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FarmerEmail",
                table: "RequestedProducts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 21, 23, 0, 0, 700, DateTimeKind.Local).AddTicks(2592), "$2b$10$Jf8x/F3cnFnk2GLJat/Pb.lLncAsViAm4qVTj5flSWNUyDWtEy9Am" });
        }
    }
}
