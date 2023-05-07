using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class AddedDishAndDishesInRestaurantTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    IdDish = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.IdDish);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantDishes",
                columns: table => new
                {
                    IdRestaurantDish = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDish = table.Column<int>(type: "int", nullable: false),
                    IdRestaurant = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantDishes", x => x.IdRestaurantDish);
                    table.ForeignKey(
                        name: "FK_RestaurantDishes_Dishes_IdDish",
                        column: x => x.IdDish,
                        principalTable: "Dishes",
                        principalColumn: "IdDish",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantDishes_Restaurants_IdRestaurant",
                        column: x => x.IdRestaurant,
                        principalTable: "Restaurants",
                        principalColumn: "IdRestaurant",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "IdDish", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Pizza", 19.99m },
                    { 2, "Calzone", 11.99m },
                    { 3, "Pierogi", 9.99m },
                    { 4, "Brat Wurst", 7.99m },
                    { 5, "Cheese Bread", 19.99m },
                    { 6, "Carbonara", 12.99m }
                });

            migrationBuilder.InsertData(
                table: "RestaurantDishes",
                columns: new[] { "IdRestaurantDish", "IdDish", "IdRestaurant" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 },
                    { 4, 4, 1 },
                    { 5, 5, 1 },
                    { 6, 6, 1 },
                    { 7, 6, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantDishes_IdDish",
                table: "RestaurantDishes",
                column: "IdDish");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantDishes_IdRestaurant",
                table: "RestaurantDishes",
                column: "IdRestaurant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantDishes");

            migrationBuilder.DropTable(
                name: "Dishes");
        }
    }
}
