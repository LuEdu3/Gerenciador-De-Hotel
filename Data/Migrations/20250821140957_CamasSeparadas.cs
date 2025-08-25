using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorHotel.Migrations
{
    /// <inheritdoc />
    public partial class CamasSeparadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeCamasCasal",
                table: "Acomodacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeCamasCasal",
                table: "Acomodacoes");
        }
    }
}
