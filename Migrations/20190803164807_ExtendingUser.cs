using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace RestApiDating.Migrations
{
    public partial class ExtendingUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Buscando",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genero",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Intereses",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Introduccion",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaConexion",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fotos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    FechaCarga = table.Column<DateTime>(nullable: false),
                    EsPrincipal = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_UserId",
                table: "Fotos",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotos");

            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Buscando",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Genero",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Intereses",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Introduccion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UltimaConexion",
                table: "Users");
        }
    }
}
