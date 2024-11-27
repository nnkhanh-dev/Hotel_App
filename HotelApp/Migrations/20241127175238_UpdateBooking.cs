using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24eaefcf-25a4-4384-bb4f-d0fe86816751");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "992b929d-5d89-4b0b-975c-8c318122d594");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Bookings");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5573275e-e552-4175-976f-24dc6d80ba3a", null, "Admin", "ADMIN" },
                    { "a770f3fc-f9e3-41eb-88a8-68f533f8496d", null, "Client", "CLIENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5573275e-e552-4175-976f-24dc6d80ba3a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a770f3fc-f9e3-41eb-88a8-68f533f8496d");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Bookings");

            migrationBuilder.AddColumn<decimal>(
                name: "Paid",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24eaefcf-25a4-4384-bb4f-d0fe86816751", null, "Admin", "ADMIN" },
                    { "992b929d-5d89-4b0b-975c-8c318122d594", null, "Client", "CLIENT" }
                });
        }
    }
}
