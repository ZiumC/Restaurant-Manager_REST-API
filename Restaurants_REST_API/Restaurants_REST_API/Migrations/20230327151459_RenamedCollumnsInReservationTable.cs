using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class RenamedCollumnsInReservationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StateOfReservation",
                table: "Reservations",
                newName: "ReservationStatus");

            migrationBuilder.RenameColumn(
                name: "GradeOfReservation",
                table: "Reservations",
                newName: "ReservationGrade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReservationStatus",
                table: "Reservations",
                newName: "StateOfReservation");

            migrationBuilder.RenameColumn(
                name: "ReservationGrade",
                table: "Reservations",
                newName: "GradeOfReservation");
        }
    }
}
