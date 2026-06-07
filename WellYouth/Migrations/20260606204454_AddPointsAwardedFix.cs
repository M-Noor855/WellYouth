using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WellYouth.Migrations
{
    /// <inheritdoc />
    public partial class AddPointsAwardedFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PointsAwarded",
                table: "HealthHabitLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsAwarded",
                table: "HealthHabitLogs");
        }
    }
}
