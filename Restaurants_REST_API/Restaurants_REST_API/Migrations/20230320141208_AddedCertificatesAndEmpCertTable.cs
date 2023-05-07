using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class AddedCertificatesAndEmpCertTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    IdCertificate = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.IdCertificate);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCertificates",
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
                    table.PrimaryKey("PK_EmployeeCertificates", x => x.IdEmployeeCertificate);
                    table.ForeignKey(
                        name: "FK_EmployeeCertificates_Certificates_IdCertificate",
                        column: x => x.IdCertificate,
                        principalTable: "Certificates",
                        principalColumn: "IdCertificate",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmployeeCertificates_Employees_IdEmployee",
                        column: x => x.IdEmployee,
                        principalTable: "Employees",
                        principalColumn: "IdEmployee",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Certificates",
                columns: new[] { "IdCertificate", "Name" },
                values: new object[] { 1, "Italian Cuisine" });

            migrationBuilder.InsertData(
                table: "Certificates",
                columns: new[] { "IdCertificate", "Name" },
                values: new object[] { 2, "Germany Cuisine" });

            migrationBuilder.InsertData(
                table: "Certificates",
                columns: new[] { "IdCertificate", "Name" },
                values: new object[] { 3, "Polish Cuisine" });

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

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertificates_IdCertificate",
                table: "EmployeeCertificates",
                column: "IdCertificate");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertificates_IdEmployee",
                table: "EmployeeCertificates",
                column: "IdEmployee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeCertificates");

            migrationBuilder.DropTable(
                name: "Certificates");
        }
    }
}
