using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class UniqeEmailAndIndexOnToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RefreshTokenHash",
                table: "Auths",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpiresAt",
                table: "Auths",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Auths",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Auths_Email",
                table: "Auths",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auths_RefreshTokenHash",
                table: "Auths",
                column: "RefreshTokenHash",
                unique: true,
                filter: "[RefreshTokenHash] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Auths_Email",
                table: "Auths");

            migrationBuilder.DropIndex(
                name: "IX_Auths_RefreshTokenHash",
                table: "Auths");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshTokenHash",
                table: "Auths",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "RefreshTokenExpiresAt",
                table: "Auths",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Auths",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
