using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Librarium.Data.Migrations
{
    /// <inheritdoc />
    public partial class V006_IsbnColumnTypeChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsbnHistorical",
                table: "Books",
                type: "text",
                nullable: true);

            migrationBuilder.Sql("UPDATE \"Books\" SET \"IsbnHistorical\" = \"Isbn\" WHERE \"Isbn\" IS NOT NULL");

            migrationBuilder.AlterColumn<string>(
                name: "Isbn",
                table: "Books",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.Sql("UPDATE \"Books\" SET \"Isbn\" = NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsbnHistorical",
                table: "Books");
        }
    }
}
