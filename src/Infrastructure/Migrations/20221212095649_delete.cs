using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_ModeratorId",
                table: "Games");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_ModeratorId",
                table: "Games",
                column: "ModeratorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_ModeratorId",
                table: "Games");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_ModeratorId",
                table: "Games",
                column: "ModeratorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
