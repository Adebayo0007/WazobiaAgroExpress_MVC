using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class sound : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FarmerRank",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 11, 22, 52, 24, 649, DateTimeKind.Local).AddTicks(6899), "$2b$10$20W47AVPTu879efE.8ee5O3MPaejtaLngjiOK.ThG5ZMQK70QzLlK" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FarmerRank",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 10, 2, 19, 50, 455, DateTimeKind.Local).AddTicks(4689), "$2b$10$LPHvoVRYrWElSsDaF/o4weKL4E5v97Ry7QtuCVU2CokzvYMissAg." });
        }
    }
}
