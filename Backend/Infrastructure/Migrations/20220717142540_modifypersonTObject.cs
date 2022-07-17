using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class modifypersonTObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "Persons",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Persons",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DateCreated", "PasswordHash", "Verified" },
                values: new object[] { new DateTime(2022, 7, 17, 15, 25, 39, 563, DateTimeKind.Local).AddTicks(7894), "$2a$11$/XmG3WuwYOB6CulLpSNATeFzZk6aE3zpwFkETMl/Sk1va.sreze5m", new DateTime(2022, 7, 17, 15, 25, 39, 561, DateTimeKind.Local).AddTicks(952) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DateCreated", "PasswordHash", "Verified" },
                values: new object[] { new DateTime(2022, 7, 16, 14, 6, 32, 145, DateTimeKind.Local).AddTicks(4745), "$2a$11$I7EZTS8.CPZWQCPbgxXEL.x0rYNpBTprgQBlrgWayFkwC6goW6F/C", new DateTime(2022, 7, 16, 14, 6, 32, 144, DateTimeKind.Local).AddTicks(2838) });
        }
    }
}
