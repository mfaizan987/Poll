using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poll.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init102 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowMultipleQuestions",
                table: "Polls",
                newName: "AllowMultipleAnswers");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Polls",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Polls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Polls",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Polls");

            migrationBuilder.RenameColumn(
                name: "AllowMultipleAnswers",
                table: "Polls",
                newName: "AllowMultipleQuestions");
        }
    }
}
