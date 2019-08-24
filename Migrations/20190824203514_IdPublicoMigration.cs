using Microsoft.EntityFrameworkCore.Migrations;

namespace RestApiDating.Migrations
{
    public partial class IdPublicoMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdPublico",
                table: "Fotos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdPublico",
                table: "Fotos");
        }
    }
}
