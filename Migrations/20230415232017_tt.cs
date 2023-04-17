using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class tt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Haspaid",
                table: "RequestedProducts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 16, 0, 20, 16, 585, DateTimeKind.Local).AddTicks(9957), "$2b$10$2FACaT03XVHZrB5bUjapougV1jwdzeo/UkzqBAkX/GDqxCgD/J1XS" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Haspaid",
                table: "RequestedProducts");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 15, 22, 53, 28, 508, DateTimeKind.Local).AddTicks(1530), "$2b$10$lrOjCL/DVkDsGLN76/.2JuU/KDgUly0MeYD/9nMS3lEEqYVas2OaK" });
        }
    }
}
