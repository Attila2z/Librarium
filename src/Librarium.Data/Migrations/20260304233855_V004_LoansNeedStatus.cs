using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Librarium.Data.Migrations
{
    /// <inheritdoc />
    public partial class V004_LoansNeedStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Loans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Populate existing loans based on ReturnDate
            // If ReturnDate is null -> Active (0), otherwise -> Returned (1)
            migrationBuilder.Sql(
                @"UPDATE ""Loans"" SET ""Status"" = CASE WHEN ""ReturnDate"" IS NULL THEN 0 ELSE 1 END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Loans");
        }
    }
}
