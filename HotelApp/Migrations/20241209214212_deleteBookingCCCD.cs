using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class deleteBookingCCCD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingCCCD");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7fd142d-9bec-4209-bb0f-88be4fd4479e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1f14bda-0adb-4a85-b6a3-e9023cc355fc");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "CCCDs",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e216cb7-c500-4c05-9f3c-19d2b36f0c56", null, "Admin", "ADMIN" },
                    { "39d51654-517e-484d-8732-b3a2f95c38eb", null, "Client", "CLIENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CCCDs_BookingId",
                table: "CCCDs",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_CCCDs_Bookings_BookingId",
                table: "CCCDs",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CCCDs_Bookings_BookingId",
                table: "CCCDs");

            migrationBuilder.DropIndex(
                name: "IX_CCCDs_BookingId",
                table: "CCCDs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e216cb7-c500-4c05-9f3c-19d2b36f0c56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39d51654-517e-484d-8732-b3a2f95c38eb");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "CCCDs");

            migrationBuilder.CreateTable(
                name: "BookingCCCD",
                columns: table => new
                {
                    BookingsId = table.Column<int>(type: "int", nullable: false),
                    CCCDsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingCCCD", x => new { x.BookingsId, x.CCCDsId });
                    table.ForeignKey(
                        name: "FK_BookingCCCD_Bookings_BookingsId",
                        column: x => x.BookingsId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingCCCD_CCCDs_CCCDsId",
                        column: x => x.CCCDsId,
                        principalTable: "CCCDs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a7fd142d-9bec-4209-bb0f-88be4fd4479e", null, "Admin", "ADMIN" },
                    { "b1f14bda-0adb-4a85-b6a3-e9023cc355fc", null, "Client", "CLIENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingCCCD_CCCDsId",
                table: "BookingCCCD",
                column: "CCCDsId");
        }
    }
}
