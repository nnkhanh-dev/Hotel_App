using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBooking3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b4a45dd-f2b1-4cb5-a7f9-7c35b2ce2b50");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bff503d0-2eb3-4d41-8ed0-44d4e4c38c27");

            migrationBuilder.AddColumn<string>(
                name: "PaymentCode",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "29c249ea-8692-4747-b001-f337244de35a", null, "Admin", "ADMIN" },
                    { "69bb3e8b-f8ae-40f8-a543-783dde9f70ab", null, "Client", "CLIENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29c249ea-8692-4747-b001-f337244de35a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69bb3e8b-f8ae-40f8-a543-783dde9f70ab");

            migrationBuilder.DropColumn(
                name: "PaymentCode",
                table: "Bookings");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8b4a45dd-f2b1-4cb5-a7f9-7c35b2ce2b50", null, "Admin", "ADMIN" },
                    { "bff503d0-2eb3-4d41-8ed0-44d4e4c38c27", null, "Client", "CLIENT" }
                });
        }
    }
}
