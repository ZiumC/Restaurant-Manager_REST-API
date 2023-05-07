using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class RenamedAllTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Address_IdAddress",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Clients_IdClient",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Restaurants_IdRestauration",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Address_IdAddress",
                table: "Restaurants");

            migrationBuilder.DropTable(
                name: "Complains");

            migrationBuilder.DropTable(
                name: "EmployeeCertificates");

            migrationBuilder.DropTable(
                name: "EmployeesInRestaurants");

            migrationBuilder.DropTable(
                name: "RestaurantDishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeTypes",
                table: "EmployeeTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dishes",
                table: "Dishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificates",
                table: "Certificates");

            migrationBuilder.RenameTable(
                name: "Restaurants",
                newName: "Restaurant");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Reservation");

            migrationBuilder.RenameTable(
                name: "EmployeeTypes",
                newName: "EmployeeType");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameTable(
                name: "Dishes",
                newName: "Dish");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Client");

            migrationBuilder.RenameTable(
                name: "Certificates",
                newName: "Certificate");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_IdAddress",
                table: "Restaurant",
                newName: "IX_Restaurant_IdAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_IdRestauration",
                table: "Reservation",
                newName: "IX_Reservation_IdRestauration");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_IdClient",
                table: "Reservation",
                newName: "IX_Reservation_IdClient");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_IdAddress",
                table: "Employee",
                newName: "IX_Employee_IdAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurant",
                table: "Restaurant",
                column: "IdRestaurant");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation",
                column: "IdReservation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeType",
                table: "EmployeeType",
                column: "IdType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "IdEmployee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dish",
                table: "Dish",
                column: "IdDish");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Client",
                table: "Client",
                column: "IdClient");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate",
                column: "IdCertificate");

            migrationBuilder.CreateTable(
                name: "Complaint",
                columns: table => new
                {
                    IdComplaint = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplainDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ComplaintStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ComplaintMessage = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    IdRestaurant = table.Column<int>(type: "int", nullable: false),
                    IdReservation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaint", x => x.IdComplaint);
                    table.ForeignKey(
                        name: "FK_Complaint_Reservation_IdReservation",
                        column: x => x.IdReservation,
                        principalTable: "Reservation",
                        principalColumn: "IdReservation",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complaint_Restaurant_IdRestaurant",
                        column: x => x.IdRestaurant,
                        principalTable: "Restaurant",
                        principalColumn: "IdRestaurant",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCertificate",
                columns: table => new
                {
                    IdEmployeeCertificate = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdEmployee = table.Column<int>(type: "int", nullable: false),
                    IdCertificate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCertificate", x => x.IdEmployeeCertificate);
                    table.ForeignKey(
                        name: "FK_EmployeeCertificate_Certificate_IdCertificate",
                        column: x => x.IdCertificate,
                        principalTable: "Certificate",
                        principalColumn: "IdCertificate",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeCertificate_Employee_IdEmployee",
                        column: x => x.IdEmployee,
                        principalTable: "Employee",
                        principalColumn: "IdEmployee",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRestaurant",
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
                    table.PrimaryKey("PK_EmployeeRestaurant", x => x.IdRestaurantWorker);
                    table.ForeignKey(
                        name: "FK_EmployeeRestaurant_Employee_IdEmployee",
                        column: x => x.IdEmployee,
                        principalTable: "Employee",
                        principalColumn: "IdEmployee",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRestaurant_EmployeeType_IdType",
                        column: x => x.IdType,
                        principalTable: "EmployeeType",
                        principalColumn: "IdType",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRestaurant_Restaurant_IdRestaurant",
                        column: x => x.IdRestaurant,
                        principalTable: "Restaurant",
                        principalColumn: "IdRestaurant",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantDishe",
                columns: table => new
                {
                    IdRestaurantDish = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDish = table.Column<int>(type: "int", nullable: false),
                    IdRestaurant = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantDishe", x => x.IdRestaurantDish);
                    table.ForeignKey(
                        name: "FK_RestaurantDishe_Dish_IdDish",
                        column: x => x.IdDish,
                        principalTable: "Dish",
                        principalColumn: "IdDish",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantDishe_Restaurant_IdRestaurant",
                        column: x => x.IdRestaurant,
                        principalTable: "Restaurant",
                        principalColumn: "IdRestaurant",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Complaint",
                columns: new[] { "IdComplaint", "ComplainDate", "ComplaintMessage", "ComplaintStatus", "IdReservation", "IdRestaurant" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zupa była za słona, obsługa była niemiła", "Canceled", 1, 1 },
                    { 2, new DateTime(2023, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Na kotlecie była mucha", "New", 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "EmployeeCertificate",
                columns: new[] { "IdEmployeeCertificate", "ExpirationDate", "IdCertificate", "IdEmployee" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 2, new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2 },
                    { 3, new DateTime(2023, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1 },
                    { 4, new DateTime(2023, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4 },
                    { 5, new DateTime(2023, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3 },
                    { 6, new DateTime(2023, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1 },
                    { 7, new DateTime(2023, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 5 }
                });

            migrationBuilder.InsertData(
                table: "EmployeeRestaurant",
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

            migrationBuilder.InsertData(
                table: "RestaurantDishe",
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
                name: "IX_Complaint_IdReservation",
                table: "Complaint",
                column: "IdReservation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Complaint_IdRestaurant",
                table: "Complaint",
                column: "IdRestaurant");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertificate_IdCertificate",
                table: "EmployeeCertificate",
                column: "IdCertificate");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertificate_IdEmployee",
                table: "EmployeeCertificate",
                column: "IdEmployee");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRestaurant_IdEmployee",
                table: "EmployeeRestaurant",
                column: "IdEmployee");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRestaurant_IdRestaurant",
                table: "EmployeeRestaurant",
                column: "IdRestaurant");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRestaurant_IdType",
                table: "EmployeeRestaurant",
                column: "IdType");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantDishe_IdDish",
                table: "RestaurantDishe",
                column: "IdDish");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantDishe_IdRestaurant",
                table: "RestaurantDishe",
                column: "IdRestaurant");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Address_IdAddress",
                table: "Employee",
                column: "IdAddress",
                principalTable: "Address",
                principalColumn: "IdAddress",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Client_IdClient",
                table: "Reservation",
                column: "IdClient",
                principalTable: "Client",
                principalColumn: "IdClient",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Restaurant_IdRestauration",
                table: "Reservation",
                column: "IdRestauration",
                principalTable: "Restaurant",
                principalColumn: "IdRestaurant",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Address_IdAddress",
                table: "Restaurant",
                column: "IdAddress",
                principalTable: "Address",
                principalColumn: "IdAddress",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Address_IdAddress",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Client_IdClient",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Restaurant_IdRestauration",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Address_IdAddress",
                table: "Restaurant");

            migrationBuilder.DropTable(
                name: "Complaint");

            migrationBuilder.DropTable(
                name: "EmployeeCertificate");

            migrationBuilder.DropTable(
                name: "EmployeeRestaurant");

            migrationBuilder.DropTable(
                name: "RestaurantDishe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurant",
                table: "Restaurant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeType",
                table: "EmployeeType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dish",
                table: "Dish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Client",
                table: "Client");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate");

            migrationBuilder.RenameTable(
                name: "Restaurant",
                newName: "Restaurants");

            migrationBuilder.RenameTable(
                name: "Reservation",
                newName: "Reservations");

            migrationBuilder.RenameTable(
                name: "EmployeeType",
                newName: "EmployeeTypes");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "Dish",
                newName: "Dishes");

            migrationBuilder.RenameTable(
                name: "Client",
                newName: "Clients");

            migrationBuilder.RenameTable(
                name: "Certificate",
                newName: "Certificates");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurant_IdAddress",
                table: "Restaurants",
                newName: "IX_Restaurants_IdAddress");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_IdRestauration",
                table: "Reservations",
                newName: "IX_Reservations_IdRestauration");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_IdClient",
                table: "Reservations",
                newName: "IX_Reservations_IdClient");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_IdAddress",
                table: "Employees",
                newName: "IX_Employees_IdAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants",
                column: "IdRestaurant");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "IdReservation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeTypes",
                table: "EmployeeTypes",
                column: "IdType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "IdEmployee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dishes",
                table: "Dishes",
                column: "IdDish");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "IdClient");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificates",
                table: "Certificates",
                column: "IdCertificate");

            migrationBuilder.CreateTable(
                name: "Complains",
                columns: table => new
                {
                    IdComplain = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdReservation = table.Column<int>(type: "int", nullable: false),
                    IdRestaurant = table.Column<int>(type: "int", nullable: false),
                    ComplainDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ComplainMessage = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    ComplainStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complains", x => x.IdComplain);
                    table.ForeignKey(
                        name: "FK_Complains_Reservations_IdReservation",
                        column: x => x.IdReservation,
                        principalTable: "Reservations",
                        principalColumn: "IdReservation",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complains_Restaurants_IdRestaurant",
                        column: x => x.IdRestaurant,
                        principalTable: "Restaurants",
                        principalColumn: "IdRestaurant",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCertificates",
                columns: table => new
                {
                    IdEmployeeCertificate = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCertificate = table.Column<int>(type: "int", nullable: false),
                    IdEmployee = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCertificates", x => x.IdEmployeeCertificate);
                    table.ForeignKey(
                        name: "FK_EmployeeCertificates_Certificates_IdCertificate",
                        column: x => x.IdCertificate,
                        principalTable: "Certificates",
                        principalColumn: "IdCertificate",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeCertificates_Employees_IdEmployee",
                        column: x => x.IdEmployee,
                        principalTable: "Employees",
                        principalColumn: "IdEmployee",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeesInRestaurants",
                columns: table => new
                {
                    IdRestaurantWorker = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmployee = table.Column<int>(type: "int", nullable: false),
                    IdRestaurant = table.Column<int>(type: "int", nullable: false),
                    IdType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesInRestaurants", x => x.IdRestaurantWorker);
                    table.ForeignKey(
                        name: "FK_EmployeesInRestaurants_Employees_IdEmployee",
                        column: x => x.IdEmployee,
                        principalTable: "Employees",
                        principalColumn: "IdEmployee",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeesInRestaurants_EmployeeTypes_IdType",
                        column: x => x.IdType,
                        principalTable: "EmployeeTypes",
                        principalColumn: "IdType",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeesInRestaurants_Restaurants_IdRestaurant",
                        column: x => x.IdRestaurant,
                        principalTable: "Restaurants",
                        principalColumn: "IdRestaurant",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Complains",
                columns: new[] { "IdComplain", "ComplainDate", "ComplainMessage", "ComplainStatus", "IdReservation", "IdRestaurant" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zupa była za słona, obsługa była niemiła", "Canceled", 1, 1 },
                    { 2, new DateTime(2023, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Na kotlecie była mucha", "New", 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "EmployeeCertificates",
                columns: new[] { "IdEmployeeCertificate", "ExpirationDate", "IdCertificate", "IdEmployee" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 2, new DateTime(2023, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2 },
                    { 3, new DateTime(2023, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1 },
                    { 4, new DateTime(2023, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4 },
                    { 5, new DateTime(2023, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3 },
                    { 6, new DateTime(2023, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1 },
                    { 7, new DateTime(2023, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 5 }
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
                name: "IX_Complains_IdReservation",
                table: "Complains",
                column: "IdReservation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Complains_IdRestaurant",
                table: "Complains",
                column: "IdRestaurant");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertificates_IdCertificate",
                table: "EmployeeCertificates",
                column: "IdCertificate");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertificates_IdEmployee",
                table: "EmployeeCertificates",
                column: "IdEmployee");

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

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantDishes_IdDish",
                table: "RestaurantDishes",
                column: "IdDish");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantDishes_IdRestaurant",
                table: "RestaurantDishes",
                column: "IdRestaurant");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Address_IdAddress",
                table: "Employees",
                column: "IdAddress",
                principalTable: "Address",
                principalColumn: "IdAddress",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Clients_IdClient",
                table: "Reservations",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "IdClient",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Restaurants_IdRestauration",
                table: "Reservations",
                column: "IdRestauration",
                principalTable: "Restaurants",
                principalColumn: "IdRestaurant",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Address_IdAddress",
                table: "Restaurants",
                column: "IdAddress",
                principalTable: "Address",
                principalColumn: "IdAddress",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
