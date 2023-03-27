using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class RenamedCollumnsInAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NoLocal",
                table: "Address",
                newName: "LocalNumber");

            migrationBuilder.RenameColumn(
                name: "NoBuilding",
                table: "Address",
                newName: "BuildingNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocalNumber",
                table: "Address",
                newName: "NoLocal");

            migrationBuilder.RenameColumn(
                name: "BuildingNumber",
                table: "Address",
                newName: "NoBuilding");
        }
    }
}
