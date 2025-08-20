using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorHotel.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarUserIdEmReservas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Reservas",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_UserId",
                table: "Reservas",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_AspNetUsers_UserId",
                table: "Reservas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_AspNetUsers_UserId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_UserId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reservas");
        }
    }
}
