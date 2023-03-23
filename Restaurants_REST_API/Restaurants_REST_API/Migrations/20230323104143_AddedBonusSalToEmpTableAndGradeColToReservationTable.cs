using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class AddedBonusSalToEmpTableAndGradeColToReservationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeOfReservation",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BonusSalary",
                table: "Employees",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 1,
                column: "BonusSalary",
                value: 150m);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 2,
                column: "BonusSalary",
                value: 150m);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 3,
                column: "BonusSalary",
                value: 150m);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 4,
                column: "BonusSalary",
                value: 150m);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 5,
                column: "BonusSalary",
                value: 150m);

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "IdReservation",
                keyValue: 3,
                column: "GradeOfReservation",
                value: 4);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeOfReservation",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BonusSalary",
                table: "Employees");
        }
    }
}
