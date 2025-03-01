using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class QuanHe1NChoBookingVaCCCD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CCCDs_Bookings_BookingId",
                table: "CCCDs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e216cb7-c500-4c05-9f3c-19d2b36f0c56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39d51654-517e-484d-8732-b3a2f95c38eb");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "CCCDs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9eeb3b1c-d25c-4438-8c4c-1e52e63d8062", null, "Client", "CLIENT" },
                    { "feba5148-fdc7-4a2c-8874-1504b5c52c55", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CCCDs_Bookings_BookingId",
                table: "CCCDs",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CCCDs_Bookings_BookingId",
                table: "CCCDs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9eeb3b1c-d25c-4438-8c4c-1e52e63d8062");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "feba5148-fdc7-4a2c-8874-1504b5c52c55");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "CCCDs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e216cb7-c500-4c05-9f3c-19d2b36f0c56", null, "Admin", "ADMIN" },
                    { "39d51654-517e-484d-8732-b3a2f95c38eb", null, "Client", "CLIENT" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CCCDs_Bookings_BookingId",
                table: "CCCDs",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");
        }
    }
}
