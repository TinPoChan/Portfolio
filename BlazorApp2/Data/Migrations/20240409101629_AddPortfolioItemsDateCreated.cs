using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp2.Migrations
{
    /// <inheritdoc />
    public partial class AddPortfolioItemsDateCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateCreated",
                table: "PortfolioItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "PortfolioItems");
        }
    }
}
