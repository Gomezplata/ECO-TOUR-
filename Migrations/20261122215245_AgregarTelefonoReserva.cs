using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoTour.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTelefonoReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Reservas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Reservas");
        }
    }
}
