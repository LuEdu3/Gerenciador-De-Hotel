using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorHotel.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirColunasDuplicadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeCamas",
                table: "Acomodacoes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeCamas",
                table: "Acomodacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
