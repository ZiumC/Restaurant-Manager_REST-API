using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class ModifiedTablesReservationAndComplains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TableNumber",
                table: "Reservations",
                newName: "HowManyPeoples");

            migrationBuilder.AddColumn<string>(
                name: "ComplainMessage",
                table: "Complains",
                type: "nvarchar(350)",
                maxLength: 350,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Complains",
                keyColumn: "IdComplain",
                keyValue: 1,
                columns: new[] { "ComplainMessage", "ComplainStatus" },
                values: new object[] { "Zupa była za słona, obsługa była niemiła", "Canceled" });

            migrationBuilder.InsertData(
                table: "Complains",
                columns: new[] { "IdComplain", "ComplainDate", "ComplainMessage", "ComplainStatus", "IdReservation", "IdRestaurant" },
                values: new object[] { 2, new DateTime(2023, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Na kotlecie była mucha", "New", 3, 1 });

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "IdReservation",
                keyValue: 2,
                column: "HowManyPeoples",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "IdReservation",
                keyValue: 3,
                column: "HowManyPeoples",
                value: 6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Complains",
                keyColumn: "IdComplain",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "ComplainMessage",
                table: "Complains");

            migrationBuilder.RenameColumn(
                name: "HowManyPeoples",
                table: "Reservations",
                newName: "TableNumber");

            migrationBuilder.UpdateData(
                table: "Complains",
                keyColumn: "IdComplain",
                keyValue: 1,
                column: "ComplainStatus",
                value: "New");

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "IdReservation",
                keyValue: 2,
                column: "TableNumber",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "IdReservation",
                keyValue: 3,
                column: "TableNumber",
                value: 1);
        }
    }
}
