using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class changedbonus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Restaurant",
                keyColumn: "IdRestaurant",
                keyValue: 1,
                column: "BonusBudget",
                value: 1230m);

            migrationBuilder.UpdateData(
                table: "Restaurant",
                keyColumn: "IdRestaurant",
                keyValue: 2,
                column: "BonusBudget",
                value: 400m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Restaurant",
                keyColumn: "IdRestaurant",
                keyValue: 1,
                column: "BonusBudget",
                value: null);

            migrationBuilder.UpdateData(
                table: "Restaurant",
                keyColumn: "IdRestaurant",
                keyValue: 2,
                column: "BonusBudget",
                value: null);
        }
    }
}
