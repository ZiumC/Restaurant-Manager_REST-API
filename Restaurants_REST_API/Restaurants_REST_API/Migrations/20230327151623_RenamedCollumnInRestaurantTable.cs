using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class RenamedCollumnInRestaurantTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StateOfRestaurant",
                table: "Restaurants",
                newName: "RestaurantStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RestaurantStatus",
                table: "Restaurants",
                newName: "StateOfRestaurant");
        }
    }
}
