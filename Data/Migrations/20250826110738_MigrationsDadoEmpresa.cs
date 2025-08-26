using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace GerenciadorHotel.Migrations
{
    /// <inheritdoc />
    public partial class MigrationsDadoEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhatsApp",
                table: "Empresas",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE Empresas SET WhatsApp = Contato");

            migrationBuilder.DropColumn(
                name: "Contato",
                table: "Empresas");

            migrationBuilder.AddColumn<int>(
                name: "AnoFundacao",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Empresas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CEP",
                table: "Empresas",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Empresas",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAtualizacao",
                table: "Empresas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescricaoBreve",
                table: "Empresas",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescricaoSobre",
                table: "Empresas",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Empresas",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Empresas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Empresas",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HorarioCheckin",
                table: "Empresas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HorarioCheckout",
                table: "Empresas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "Empresas",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "Empresas",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeResumido",
                table: "Empresas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "Empresas",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slogan",
                table: "Empresas",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Empresas",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "Empresas",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Empresas",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AltText",
                table: "EmpresaFotos",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "EmpresaFotos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "EmpresaFotos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Ordem",
                table: "EmpresaFotos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "EmpresaFotos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmpresaPremios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Ano = table.Column<int>(type: "int", nullable: true),
                    Instituicao = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Icone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpresaPremios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpresaPremios_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmpresaServicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Icone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpresaServicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpresaServicos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaPremios_EmpresaId",
                table: "EmpresaPremios",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaServicos_EmpresaId",
                table: "EmpresaServicos",
                column: "EmpresaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpresaPremios");

            migrationBuilder.DropTable(
                name: "EmpresaServicos");

            migrationBuilder.DropColumn(
                name: "AnoFundacao",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "CEP",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "DataAtualizacao",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "DescricaoBreve",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "DescricaoSobre",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "HorarioCheckin",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "HorarioCheckout",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "NomeResumido",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Slogan",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "AltText",
                table: "EmpresaFotos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "EmpresaFotos");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "EmpresaFotos");

            migrationBuilder.DropColumn(
                name: "Ordem",
                table: "EmpresaFotos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "EmpresaFotos");

            migrationBuilder.DropColumn(
                name: "WhatsApp",
                table: "Empresas");
        }
    }
}
