using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class modifypersonSurname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DateCreated", "PasswordHash", "Verified" },
                values: new object[] { new DateTime(2022, 7, 17, 16, 41, 1, 43, DateTimeKind.Local).AddTicks(8598), "$2a$11$SolhwFbAHYcJabwMa8DhWePc95MrGvmPYK2hOpYggPc85iTMQ4AD2", new DateTime(2022, 7, 17, 16, 41, 1, 42, DateTimeKind.Local).AddTicks(1658) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DateCreated", "PasswordHash", "Verified" },
                values: new object[] { new DateTime(2022, 7, 17, 15, 25, 39, 563, DateTimeKind.Local).AddTicks(7894), "$2a$11$/XmG3WuwYOB6CulLpSNATeFzZk6aE3zpwFkETMl/Sk1va.sreze5m", new DateTime(2022, 7, 17, 15, 25, 39, 561, DateTimeKind.Local).AddTicks(952) });
        }
    }
}
