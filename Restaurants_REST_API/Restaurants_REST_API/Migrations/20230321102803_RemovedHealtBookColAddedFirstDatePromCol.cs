using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurants_REST_API.Migrations
{
    public partial class RemovedHealtBookColAddedFirstDatePromCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHealthBook",
                table: "Employees");

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstPromotionChefDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NoLocal",
                table: "Address",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.UpdateData(
                table: "Address",
                keyColumn: "IdAddress",
                keyValue: 4,
                column: "NoLocal",
                value: null);

            migrationBuilder.UpdateData(
                table: "Address",
                keyColumn: "IdAddress",
                keyValue: 6,
                column: "NoLocal",
                value: null);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 1,
                column: "FirstPromotionChefDate",
                value: new DateTime(2000, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstPromotionChefDate",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "IsHealthBook",
                table: "Employees",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "NoLocal",
                table: "Address",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Address",
                keyColumn: "IdAddress",
                keyValue: 4,
                column: "NoLocal",
                value: "");

            migrationBuilder.UpdateData(
                table: "Address",
                keyColumn: "IdAddress",
                keyValue: 6,
                column: "NoLocal",
                value: "A1");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 1,
                column: "IsHealthBook",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 2,
                column: "IsHealthBook",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 3,
                column: "IsHealthBook",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 4,
                column: "IsHealthBook",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "IdEmployee",
                keyValue: 5,
                column: "IsHealthBook",
                value: "Y");
        }
    }
}
