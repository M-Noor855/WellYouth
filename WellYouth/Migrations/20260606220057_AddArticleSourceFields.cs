using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellYouth.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleSourceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Articles",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceUrl",
                table: "Articles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "SourceUrl",
                table: "Articles");
        }
    }
}
