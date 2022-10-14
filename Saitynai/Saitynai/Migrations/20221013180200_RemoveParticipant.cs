using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saitynai.Migrations
{
    public partial class RemoveParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Participant",
                table: "Registrations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Participant",
                table: "Registrations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
