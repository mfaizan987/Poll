using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poll.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PollId",
                table: "PollVotes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "VoterId",
                table: "PollVotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PollVotes_PollId",
                table: "PollVotes",
                column: "PollId");

            migrationBuilder.AddForeignKey(
                name: "FK_PollVotes_Polls_PollId",
                table: "PollVotes",
                column: "PollId",
                principalTable: "Polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollVotes_Polls_PollId",
                table: "PollVotes");

            migrationBuilder.DropIndex(
                name: "IX_PollVotes_PollId",
                table: "PollVotes");

            migrationBuilder.DropColumn(
                name: "PollId",
                table: "PollVotes");

            migrationBuilder.DropColumn(
                name: "VoterId",
                table: "PollVotes");
        }
    }
}
