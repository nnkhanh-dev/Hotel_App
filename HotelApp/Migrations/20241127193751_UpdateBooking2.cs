using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBooking2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5573275e-e552-4175-976f-24dc6d80ba3a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a770f3fc-f9e3-41eb-88a8-68f533f8496d");

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
                    { "8b4a45dd-f2b1-4cb5-a7f9-7c35b2ce2b50", null, "Admin", "ADMIN" },
                    { "bff503d0-2eb3-4d41-8ed0-44d4e4c38c27", null, "Client", "CLIENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b4a45dd-f2b1-4cb5-a7f9-7c35b2ce2b50");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bff503d0-2eb3-4d41-8ed0-44d4e4c38c27");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Bookings");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5573275e-e552-4175-976f-24dc6d80ba3a", null, "Admin", "ADMIN" },
                    { "a770f3fc-f9e3-41eb-88a8-68f533f8496d", null, "Client", "CLIENT" }
                });
        }
    }
}
