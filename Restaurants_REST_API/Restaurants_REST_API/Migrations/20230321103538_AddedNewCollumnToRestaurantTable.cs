using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class AddedNewCollumnToRestaurantTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateOfRestaurant",
                table: "Restaurants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "IdRestaurant",
                keyValue: 1,
                column: "StateOfRestaurant",
                value: "Working");

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "IdRestaurant",
                keyValue: 2,
                column: "StateOfRestaurant",
                value: "Under construction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateOfRestaurant",
                table: "Restaurants");
        }
    }
}
