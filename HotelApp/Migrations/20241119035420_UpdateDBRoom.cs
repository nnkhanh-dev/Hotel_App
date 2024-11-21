using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "350dfdb1-0a14-42bd-ab57-a9855f151da4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bd05452-0d6f-4144-a0c2-1c04aac7f0ff");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24eaefcf-25a4-4384-bb4f-d0fe86816751", null, "Admin", "ADMIN" },
                    { "992b929d-5d89-4b0b-975c-8c318122d594", null, "Client", "CLIENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24eaefcf-25a4-4384-bb4f-d0fe86816751");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "992b929d-5d89-4b0b-975c-8c318122d594");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "350dfdb1-0a14-42bd-ab57-a9855f151da4", null, "Admin", "ADMIN" },
                    { "7bd05452-0d6f-4144-a0c2-1c04aac7f0ff", null, "Client", "CLIENT" }
                });
        }
    }
}
