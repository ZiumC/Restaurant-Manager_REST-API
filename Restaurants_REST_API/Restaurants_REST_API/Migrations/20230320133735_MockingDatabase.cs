using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class MockingDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "IdAddress", "City", "NoBuilding", "NoLocal", "Street" },
                values: new object[,]
                {
                    { 1, "Warsaw", "11", "1", "John Paul II" },
                    { 2, "Warsaw", "1", "2", "John Paul II" },
                    { 3, "Warsaw", "78", "32A", "Stawki" },
                    { 4, "Warsaw", "14", "", "Romualda" },
                    { 5, "Warsaw", "B2", "21", "Skret" },
                    { 6, "Warsaw", "A1", "A1", "Koszyk" },
                    { 7, "Warsaw", "21", "37", "John Paul II" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "IdClient", "IsBusinessman", "Name" },
                values: new object[,]
                {
                    { 1, "N", "Jan" },
                    { 2, "N", "Michal" },
                    { 3, "Y", "Joanna" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "IdEmployee", "HiredDate", "IdAddress", "IsHealthBook", "IsOwner", "Name", "PESEL", "Salary", "Surname" },
                values: new object[,]
                {
                    { 1, new DateTime(1999, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Y", "Y", "Michal", "11122233344", 3421m, "Nowak" },
                    { 2, new DateTime(2002, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Y", "N", "Kacper", "22233344455", 2217m, "Kawka" },
                    { 3, new DateTime(2004, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Y", "N", "Janusz", "33344455566", 2213m, "Jogurt" },
                    { 4, new DateTime(2005, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Y", "N", "Bozena", "44455566677", 2613m, "Lawenda" },
                    { 5, new DateTime(2010, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Y", "N", "Joanna", "55566677788", 2913m, "Skrzynka" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "IdRestaurant", "IdAddress", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Pod Lasem" },
                    { 2, 2, "Zapiecek" }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "IdReservation", "IdClient", "IdRestauration", "ReservationDate", "TableNumber" },
                values: new object[] { 1, 1, 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "IdReservation", "IdClient", "IdRestauration", "ReservationDate", "TableNumber" },
                values: new object[] { 2, 3, 1, new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "IdReservation", "IdClient", "IdRestauration", "ReservationDate", "TableNumber" },
                values: new object[] { 3, 2, 1, new DateTime(2023, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "IdReservation",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "IdReservation",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "IdReservation",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "IdRestaurant",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "IdAddress",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "IdAddress",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "IdAddress",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "IdAddress",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "IdAddress",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "IdAddress",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "IdClient",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "IdClient",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "IdClient",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "IdRestaurant",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "IdAddress",
                keyValue: 1);
        }
    }
}
