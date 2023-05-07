using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class FixedTypoInTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantDishe_Dish_IdDish",
                table: "RestaurantDishe");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantDishe_Restaurant_IdRestaurant",
                table: "RestaurantDishe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RestaurantDishe",
                table: "RestaurantDishe");

            migrationBuilder.RenameTable(
                name: "RestaurantDishe",
                newName: "RestaurantDish");

            migrationBuilder.RenameIndex(
                name: "IX_RestaurantDishe_IdRestaurant",
                table: "RestaurantDish",
                newName: "IX_RestaurantDish_IdRestaurant");

            migrationBuilder.RenameIndex(
                name: "IX_RestaurantDishe_IdDish",
                table: "RestaurantDish",
                newName: "IX_RestaurantDish_IdDish");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestaurantDish",
                table: "RestaurantDish",
                column: "IdRestaurantDish");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantDish_Dish_IdDish",
                table: "RestaurantDish",
                column: "IdDish",
                principalTable: "Dish",
                principalColumn: "IdDish",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantDish_Restaurant_IdRestaurant",
                table: "RestaurantDish",
                column: "IdRestaurant",
                principalTable: "Restaurant",
                principalColumn: "IdRestaurant",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantDish_Dish_IdDish",
                table: "RestaurantDish");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantDish_Restaurant_IdRestaurant",
                table: "RestaurantDish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RestaurantDish",
                table: "RestaurantDish");

            migrationBuilder.RenameTable(
                name: "RestaurantDish",
                newName: "RestaurantDishe");

            migrationBuilder.RenameIndex(
                name: "IX_RestaurantDish_IdRestaurant",
                table: "RestaurantDishe",
                newName: "IX_RestaurantDishe_IdRestaurant");

            migrationBuilder.RenameIndex(
                name: "IX_RestaurantDish_IdDish",
                table: "RestaurantDishe",
                newName: "IX_RestaurantDishe_IdDish");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestaurantDishe",
                table: "RestaurantDishe",
                column: "IdRestaurantDish");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantDishe_Dish_IdDish",
                table: "RestaurantDishe",
                column: "IdDish",
                principalTable: "Dish",
                principalColumn: "IdDish",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantDishe_Restaurant_IdRestaurant",
                table: "RestaurantDishe",
                column: "IdRestaurant",
                principalTable: "Restaurant",
                principalColumn: "IdRestaurant",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
