using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendaKether.Infrastructure.Migrations;

/// <inheritdoc />
public partial class _20261117220549_InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Contatos",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Nome = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                Endereco = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Telefone = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                Email = table.Column<string>(type: "TEXT", maxLength: 254, nullable: false),
                Excluido = table.Column<bool>(type: "INTEGER", nullable: false),
                CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Contatos", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Contatos_Email_Unique",
            table: "Contatos",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Contatos_Nome",
            table: "Contatos",
            column: "Nome");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Contatos");
    }
}
