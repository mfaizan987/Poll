using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Poll.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollVotes_PollOptions_PollOptionId",
                table: "PollVotes");

            migrationBuilder.AddForeignKey(
                name: "FK_PollVotes_PollOptions_PollOptionId",
                table: "PollVotes",
                column: "PollOptionId",
                principalTable: "PollOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PollVotes_PollOptions_PollOptionId",
                table: "PollVotes");

            migrationBuilder.AddForeignKey(
                name: "FK_PollVotes_PollOptions_PollOptionId",
                table: "PollVotes",
                column: "PollOptionId",
                principalTable: "PollOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
