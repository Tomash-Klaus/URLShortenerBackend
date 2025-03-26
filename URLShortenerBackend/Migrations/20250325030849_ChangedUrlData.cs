using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShortenerBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUrlData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "UrlDatas");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "UrlDatas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_UrlDatas_CreatedBy",
                table: "UrlDatas",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_UrlDatas_AspNetUsers_CreatedBy",
                table: "UrlDatas",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UrlDatas_AspNetUsers_CreatedBy",
                table: "UrlDatas");

            migrationBuilder.DropIndex(
                name: "IX_UrlDatas_CreatedBy",
                table: "UrlDatas");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "UrlDatas",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "UrlDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
