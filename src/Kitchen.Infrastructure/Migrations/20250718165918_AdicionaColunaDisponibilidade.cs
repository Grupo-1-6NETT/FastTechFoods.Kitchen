using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kitchen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaColunaDisponibilidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormaDeEntrega",
                table: "Pedidos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Justificativa",
                table: "Pedidos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormaDeEntrega",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Justificativa",
                table: "Pedidos");
        }
    }
}
