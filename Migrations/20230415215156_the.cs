using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class the : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 15, 22, 51, 55, 704, DateTimeKind.Local).AddTicks(7088), "$2b$10$fRBTpjvkMUm6/.rLaJJaWuhhIG49hKXrQa7dRrpVLP72PiIkBzTgS" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2023, 4, 15, 22, 48, 57, 86, DateTimeKind.Local).AddTicks(3632), "$2b$10$V4W9NoI68yNI0LJgUnoDp.l.YbtF6jNffj2G8wMfpytRyfPiWBFnS" });
        }
    }
}
