using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Migrations
{
    /// <inheritdoc />
    public partial class AddClienteTipoAndUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoCliente",
                table: "Cliente",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Normal");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Cliente",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_UsuarioId",
                table: "Cliente",
                column: "UsuarioId",
                unique: true,
                filter: "[UsuarioId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Usuario_UsuarioId",
                table: "Cliente",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Usuario_UsuarioId",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_UsuarioId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "TipoCliente",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Cliente");
        }
    }
}
