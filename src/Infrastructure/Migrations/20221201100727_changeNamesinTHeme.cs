using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeNamesinTHeme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThemeConfiguration_NavbarThemeColor",
                table: "Games",
                newName: "ThemeConfiguration_SecondaryThemeColor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThemeConfiguration_SecondaryThemeColor",
                table: "Games",
                newName: "ThemeConfiguration_NavbarThemeColor");
        }
    }
}
