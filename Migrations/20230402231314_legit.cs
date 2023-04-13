using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class legit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "IsRegistered", "Password" },
                values: new object[] { new DateTime(2023, 4, 3, 0, 13, 14, 508, DateTimeKind.Local).AddTicks(3764), true, "$2b$10$EFRIb6wYlf85xGXxyryPWeXaWEWoTP4xFsM8a5wSHP0gvQAKpH2tG" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "IsRegistered", "Password" },
                values: new object[] { new DateTime(2023, 4, 3, 0, 0, 14, 844, DateTimeKind.Local).AddTicks(9780), false, "$2b$10$hDYX4cBQ5byQyEe0.M//g.8o1atea6pZ/t7BQHxsadN0OepnoRLAO" });
        }
    }
}
