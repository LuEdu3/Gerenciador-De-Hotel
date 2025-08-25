using Microsoft.EntityFrameworkCore.Migrations;

namespace GerenciadorHotel.Data.Migrations
{
    public partial class AdicionarCamasSolteiroCasalAcomodacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeCamasSolteiro",
                table: "Acomodacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeCamasSolteiro",
                table: "Acomodacoes");
        }
    }
}
