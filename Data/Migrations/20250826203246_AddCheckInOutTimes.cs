using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorHotel.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckInOutTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Adicionar colunas como NULL para evitar problemas com default no MySQL
            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraCheckIn",
                table: "Acomodacoes",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraCheckOut",
                table: "Acomodacoes",
                type: "time(6)",
                nullable: true);

            // 2) Preencher valores padrão nas linhas existentes
            migrationBuilder.Sql("UPDATE `Acomodacoes` SET `HoraCheckIn` = '14:00:00' WHERE `HoraCheckIn` IS NULL;");
            migrationBuilder.Sql("UPDATE `Acomodacoes` SET `HoraCheckOut` = '12:00:00' WHERE `HoraCheckOut` IS NULL;");

            // 3) Alterar para NOT NULL
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraCheckIn",
                table: "Acomodacoes",
                type: "time(6)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "HoraCheckOut",
                table: "Acomodacoes",
                type: "time(6)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoraCheckIn",
                table: "Acomodacoes");

            migrationBuilder.DropColumn(
                name: "HoraCheckOut",
                table: "Acomodacoes");
        }
    }
}
