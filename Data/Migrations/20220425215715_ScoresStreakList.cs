using Microsoft.EntityFrameworkCore.Migrations;

namespace ScoreStore.Data.Migrations
{
    public partial class ScoresStreakList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StreakList",
                table: "Scores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StreakList",
                table: "Scores");
        }
    }
}
