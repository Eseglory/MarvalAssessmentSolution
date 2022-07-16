using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddpersonsObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Persons_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DateCreated", "PasswordHash", "Verified" },
                values: new object[] { new DateTime(2022, 7, 16, 14, 6, 32, 145, DateTimeKind.Local).AddTicks(4745), "$2a$11$I7EZTS8.CPZWQCPbgxXEL.x0rYNpBTprgQBlrgWayFkwC6goW6F/C", new DateTime(2022, 7, 16, 14, 6, 32, 144, DateTimeKind.Local).AddTicks(2838) });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_AccountId",
                table: "Persons",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DateCreated", "PasswordHash", "Verified" },
                values: new object[] { new DateTime(2022, 7, 16, 13, 52, 53, 576, DateTimeKind.Local).AddTicks(5116), "$2a$11$5aqcFyUuTQMSZ2jlDD.f7Ozo.UktzJGkxZiGbJ1BTcxe1iQjYGeTm", new DateTime(2022, 7, 16, 13, 52, 53, 574, DateTimeKind.Local).AddTicks(8914) });
        }
    }
}
