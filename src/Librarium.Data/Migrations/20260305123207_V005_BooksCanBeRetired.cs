using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Librarium.Data.Migrations
{
    /// <inheritdoc />
    public partial class V005_BooksCanBeRetired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRetired",
                table: "Books",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRetired",
                table: "Books");
        }
    }
}
