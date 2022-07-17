using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class modifypersonSurname_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DateCreated", "PasswordHash", "Verified" },
                values: new object[] { new DateTime(2022, 7, 17, 17, 17, 23, 244, DateTimeKind.Local).AddTicks(14), "$2a$11$wXvTM6Z0usP7NSzNrHYZsOSwLuw9san.hbn9pED/82RpzgKylcNWq", new DateTime(2022, 7, 17, 17, 17, 23, 241, DateTimeKind.Local).AddTicks(6102) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DateCreated", "PasswordHash", "Verified" },
                values: new object[] { new DateTime(2022, 7, 17, 16, 41, 1, 43, DateTimeKind.Local).AddTicks(8598), "$2a$11$SolhwFbAHYcJabwMa8DhWePc95MrGvmPYK2hOpYggPc85iTMQ4AD2", new DateTime(2022, 7, 17, 16, 41, 1, 42, DateTimeKind.Local).AddTicks(1658) });
        }
    }
}
