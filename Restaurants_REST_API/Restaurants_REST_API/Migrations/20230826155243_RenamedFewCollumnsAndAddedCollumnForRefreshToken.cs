using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class RenamedFewCollumnsAndAddedCollumnForRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Restaurant_IdRestauration",
                table: "Reservation");

            migrationBuilder.RenameColumn(
                name: "LoginAttemps",
                table: "User",
                newName: "LoginAttempts");

            migrationBuilder.RenameColumn(
                name: "IdRestauration",
                table: "Reservation",
                newName: "IdRestaurant");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_IdRestauration",
                table: "Reservation",
                newName: "IX_Reservation_IdRestaurant");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "User",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Restaurant_IdRestaurant",
                table: "Reservation",
                column: "IdRestaurant",
                principalTable: "Restaurant",
                principalColumn: "IdRestaurant",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Restaurant_IdRestaurant",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "LoginAttempts",
                table: "User",
                newName: "LoginAttemps");

            migrationBuilder.RenameColumn(
                name: "IdRestaurant",
                table: "Reservation",
                newName: "IdRestauration");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_IdRestaurant",
                table: "Reservation",
                newName: "IX_Reservation_IdRestauration");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Restaurant_IdRestauration",
                table: "Reservation",
                column: "IdRestauration",
                principalTable: "Restaurant",
                principalColumn: "IdRestaurant",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
