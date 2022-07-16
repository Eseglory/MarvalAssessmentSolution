using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addpassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DateCreated", "PasswordHash", "Verified" },
                values: new object[] { new DateTime(2022, 7, 16, 13, 52, 53, 576, DateTimeKind.Local).AddTicks(5116), "$2a$11$5aqcFyUuTQMSZ2jlDD.f7Ozo.UktzJGkxZiGbJ1BTcxe1iQjYGeTm", new DateTime(2022, 7, 16, 13, 52, 53, 574, DateTimeKind.Local).AddTicks(8914) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DateCreated", "PasswordHash", "Verified" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new DateTime(2022, 7, 16, 13, 3, 35, 900, DateTimeKind.Local).AddTicks(8204) });
        }
    }
}
