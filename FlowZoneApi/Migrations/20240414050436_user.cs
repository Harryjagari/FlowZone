using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowZoneApi.Migrations
{
    /// <inheritdoc />
    public partial class user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "AdminId",
                keyValue: new Guid("232827fe-8766-42f9-acd6-905b6522913a"));

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "AdminId", "AdminPassword", "AdminUserName" },
                values: new object[] { new Guid("851e3a91-14a0-41e4-bfe1-d0dc1265d153"), "Harendra123", "Harendra" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "AdminId",
                keyValue: new Guid("851e3a91-14a0-41e4-bfe1-d0dc1265d153"));

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "AdminId", "AdminPassword", "AdminUserName" },
                values: new object[] { new Guid("232827fe-8766-42f9-acd6-905b6522913a"), "Harendra123", "Harendra" });
        }
    }
}
