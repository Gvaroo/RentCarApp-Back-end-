using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarApp.Migrations
{
    /// <inheritdoc />
    public partial class updatedRelationshipWithRentedCarAndCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RentedCars_CarId",
                table: "RentedCars");

            migrationBuilder.CreateIndex(
                name: "IX_RentedCars_CarId",
                table: "RentedCars",
                column: "CarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RentedCars_CarId",
                table: "RentedCars");

            migrationBuilder.CreateIndex(
                name: "IX_RentedCars_CarId",
                table: "RentedCars",
                column: "CarId",
                unique: true);
        }
    }
}
