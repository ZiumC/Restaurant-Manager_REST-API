using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class AddedComplainTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Complains",
                columns: table => new
                {
                    IdComplain = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplainDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusOfComplain = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdRestaurant = table.Column<int>(type: "int", nullable: false),
                    IdReservation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complains", x => x.IdComplain);
                    table.ForeignKey(
                        name: "FK_Complains_Reservations_IdReservation",
                        column: x => x.IdReservation,
                        principalTable: "Reservations",
                        principalColumn: "IdReservation",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Complains_Restaurants_IdRestaurant",
                        column: x => x.IdRestaurant,
                        principalTable: "Restaurants",
                        principalColumn: "IdRestaurant",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Complains",
                columns: new[] { "IdComplain", "ComplainDate", "IdReservation", "IdRestaurant", "StatusOfComplain" },
                values: new object[] { 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "New" });

            migrationBuilder.CreateIndex(
                name: "IX_Complains_IdReservation",
                table: "Complains",
                column: "IdReservation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Complains_IdRestaurant",
                table: "Complains",
                column: "IdRestaurant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Complains");
        }
    }
}
