using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poll.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init105 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollVotes_PollOptions_PollOptionId",
                table: "PollVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PollVotes_Polls_PollId",
                table: "PollVotes");

            migrationBuilder.DropIndex(
                name: "IX_PollVotes_PollId",
                table: "PollVotes");

            migrationBuilder.DropColumn(
                name: "PollId",
                table: "PollVotes");

            migrationBuilder.AddColumn<Guid>(
                name: "PollEntityId",
                table: "PollVotes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PollVotes_PollEntityId",
                table: "PollVotes",
                column: "PollEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PollVotes_PollOptions_PollOptionId",
                table: "PollVotes",
                column: "PollOptionId",
                principalTable: "PollOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PollVotes_Polls_PollEntityId",
                table: "PollVotes",
                column: "PollEntityId",
                principalTable: "Polls",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollVotes_PollOptions_PollOptionId",
                table: "PollVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PollVotes_Polls_PollEntityId",
                table: "PollVotes");

            migrationBuilder.DropIndex(
                name: "IX_PollVotes_PollEntityId",
                table: "PollVotes");

            migrationBuilder.DropColumn(
                name: "PollEntityId",
                table: "PollVotes");

            migrationBuilder.AddColumn<Guid>(
                name: "PollId",
                table: "PollVotes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PollVotes_PollId",
                table: "PollVotes",
                column: "PollId");

            migrationBuilder.AddForeignKey(
                name: "FK_PollVotes_PollOptions_PollOptionId",
                table: "PollVotes",
                column: "PollOptionId",
                principalTable: "PollOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PollVotes_Polls_PollId",
                table: "PollVotes",
                column: "PollId",
                principalTable: "Polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
