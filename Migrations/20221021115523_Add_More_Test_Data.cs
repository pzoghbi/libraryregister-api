using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryRegister.Migrations
{
    public partial class Add_More_Test_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[] { 1001, "default@email.com", "Default User" });

            migrationBuilder.InsertData(
                table: "Leasing",
                columns: new[] { "Id", "BookId", "LeaseDate", "ReturnDate", "UserId" },
                values: new object[] { 1, 1, new DateTime(2022, 10, 20, 11, 55, 23, 409, DateTimeKind.Utc).AddTicks(907), null, 1001 });

            migrationBuilder.InsertData(
                table: "Leasing",
                columns: new[] { "Id", "BookId", "LeaseDate", "ReturnDate", "UserId" },
                values: new object[] { 2, 2, new DateTime(2022, 10, 20, 11, 55, 23, 409, DateTimeKind.Utc).AddTicks(911), null, 1001 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Leasing",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Leasing",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1001);
        }
    }
}
