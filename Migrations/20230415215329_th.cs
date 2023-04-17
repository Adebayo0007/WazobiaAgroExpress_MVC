using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agro_Express.Migrations
{
    public partial class th : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Haspaid", "Password" },
                values: new object[] { new DateTime(2023, 4, 15, 22, 53, 28, 508, DateTimeKind.Local).AddTicks(1530), true, "$2b$10$lrOjCL/DVkDsGLN76/.2JuU/KDgUly0MeYD/9nMS3lEEqYVas2OaK" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "37846734-732e-4149-8cec-6f43d1eb3f60",
                columns: new[] { "DateCreated", "Haspaid", "Password" },
                values: new object[] { new DateTime(2023, 4, 15, 22, 51, 55, 704, DateTimeKind.Local).AddTicks(7088), false, "$2b$10$fRBTpjvkMUm6/.rLaJJaWuhhIG49hKXrQa7dRrpVLP72PiIkBzTgS" });
        }
    }
}
