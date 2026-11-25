using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendaKether.Infrastructure.Migrations;

/// <inheritdoc />
public partial class _20261125192719_AdicionaUsuario : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Usuarios",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Nome = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                Email = table.Column<string>(type: "TEXT", maxLength: 254, nullable: false),
                SenhaHash = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Excluido = table.Column<bool>(type: "INTEGER", nullable: false),
                CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Usuarios", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Usuarios_Email_Unique",
            table: "Usuarios",
            column: "Email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Usuarios");
    }
}
