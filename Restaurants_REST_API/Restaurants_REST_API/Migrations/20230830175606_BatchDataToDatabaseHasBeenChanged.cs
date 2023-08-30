using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class BatchDataToDatabaseHasBeenChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Complaint",
                keyColumn: "IdComplaint",
                keyValue: 1,
                column: "ComplaintStatus",
                value: "REJECTED");

            migrationBuilder.UpdateData(
                table: "Complaint",
                keyColumn: "IdComplaint",
                keyValue: 2,
                column: "ComplaintStatus",
                value: "NEW");

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "IdReservation",
                keyValue: 1,
                columns: new[] { "ReservationDate", "ReservationStatus" },
                values: new object[] { new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "NEW" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "IdReservation",
                keyValue: 2,
                column: "ReservationStatus",
                value: "CANCELED");

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "IdReservation",
                keyValue: 3,
                column: "ReservationStatus",
                value: "RATED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Complaint",
                keyColumn: "IdComplaint",
                keyValue: 1,
                column: "ComplaintStatus",
                value: "Canceled");

            migrationBuilder.UpdateData(
                table: "Complaint",
                keyColumn: "IdComplaint",
                keyValue: 2,
                column: "ComplaintStatus",
                value: "New");

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "IdReservation",
                keyValue: 1,
                columns: new[] { "ReservationDate", "ReservationStatus" },
                values: new object[] { new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "IdReservation",
                keyValue: 2,
                column: "ReservationStatus",
                value: "Canceled");

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "IdReservation",
                keyValue: 3,
                column: "ReservationStatus",
                value: "Finished");
        }
    }
}
