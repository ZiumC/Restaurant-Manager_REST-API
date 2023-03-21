using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class AddedNewCollumnToResTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateOfReservation",
                table: "Reservations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "IdReservation",
                keyValue: 1,
                column: "StateOfReservation",
                value: "New");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "IdReservation",
                keyValue: 2,
                column: "StateOfReservation",
                value: "Canceled");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "IdReservation",
                keyValue: 3,
                column: "StateOfReservation",
                value: "Finished");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateOfReservation",
                table: "Reservations");
        }
    }
}
