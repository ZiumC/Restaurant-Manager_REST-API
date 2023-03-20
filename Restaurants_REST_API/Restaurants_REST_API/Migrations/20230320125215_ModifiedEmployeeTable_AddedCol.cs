using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class ModifiedEmployeeTable_AddedCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsOwner",
                table: "Employees",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "Employees");
        }
    }
}
