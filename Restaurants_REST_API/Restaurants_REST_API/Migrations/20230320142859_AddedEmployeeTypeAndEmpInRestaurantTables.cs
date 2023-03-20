using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class AddedEmployeeTypeAndEmpInRestaurantTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeTypes",
                columns: table => new
                {
                    IdType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTypes", x => x.IdType);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesInRestaurants",
                columns: table => new
                {
                    IdRestaurantWorker = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmployee = table.Column<int>(type: "int", nullable: false),
                    IdType = table.Column<int>(type: "int", nullable: false),
                    IdRestaurant = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesInRestaurants", x => x.IdRestaurantWorker);
                    table.ForeignKey(
                        name: "FK_EmployeesInRestaurants_Employees_IdEmployee",
                        column: x => x.IdEmployee,
                        principalTable: "Employees",
                        principalColumn: "IdEmployee",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmployeesInRestaurants_EmployeeTypes_IdType",
                        column: x => x.IdType,
                        principalTable: "EmployeeTypes",
                        principalColumn: "IdType",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmployeesInRestaurants_Restaurants_IdRestaurant",
                        column: x => x.IdRestaurant,
                        principalTable: "Restaurants",
                        principalColumn: "IdRestaurant",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "EmployeeTypes",
                columns: new[] { "IdType", "Name" },
                values: new object[,]
                {
                    { 1, "Owner" },
                    { 2, "Chef" },
                    { 3, "Chef helper" },
                    { 4, "Waiter" }
                });

            migrationBuilder.InsertData(
                table: "EmployeesInRestaurants",
                columns: new[] { "IdRestaurantWorker", "IdEmployee", "IdRestaurant", "IdType" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 2, 1, 2 },
                    { 3, 3, 1, 3 },
                    { 4, 4, 1, 3 },
                    { 5, 5, 1, 4 },
                    { 6, 1, 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesInRestaurants_IdEmployee",
                table: "EmployeesInRestaurants",
                column: "IdEmployee");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesInRestaurants_IdRestaurant",
                table: "EmployeesInRestaurants",
                column: "IdRestaurant");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesInRestaurants_IdType",
                table: "EmployeesInRestaurants",
                column: "IdType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeesInRestaurants");

            migrationBuilder.DropTable(
                name: "EmployeeTypes");
        }
    }
}
