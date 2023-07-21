using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarApp.Migrations
{
	/// <inheritdoc />
	public partial class ChangedExpressDBToDeveloper : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Brands",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Brands", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Transmissions",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Transmissions", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "users",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
					PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
					PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
					Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Number = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "carModels",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					BrandId = table.Column<int>(type: "int", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_carModels", x => x.Id);
					table.ForeignKey(
						name: "FK_carModels_Brands_BrandId",
						column: x => x.BrandId,
						principalTable: "Brands",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "cars",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Year = table.Column<int>(type: "int", nullable: false),
					Price = table.Column<int>(type: "int", nullable: false),
					City = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Capacity = table.Column<int>(type: "int", nullable: false),
					FuelCapacity = table.Column<int>(type: "int", nullable: false),
					Latitude = table.Column<int>(type: "int", nullable: true),
					longtitude = table.Column<int>(type: "int", nullable: true),
					Likes = table.Column<int>(type: "int", nullable: false),
					IsRented = table.Column<bool>(type: "bit", nullable: false),
					UserId = table.Column<int>(type: "int", nullable: false),
					BrandId = table.Column<int>(type: "int", nullable: false),
					ModelId = table.Column<int>(type: "int", nullable: false),
					TransmissionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_cars", x => x.Id);
					table.ForeignKey(
						name: "FK_cars_Brands_BrandId",
						column: x => x.BrandId,
						principalTable: "Brands",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_cars_Transmissions_TransmissionId",
						column: x => x.TransmissionId,
						principalTable: "Transmissions",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_cars_carModels_ModelId",
						column: x => x.ModelId,
						principalTable: "carModels",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_cars_users_UserId",
						column: x => x.UserId,
						principalTable: "users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "CarImages",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
					CarsId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CarImages", x => x.Id);
					table.ForeignKey(
						name: "FK_CarImages_cars_CarsId",
						column: x => x.CarsId,
						principalTable: "cars",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "RentedCars",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UserId = table.Column<int>(type: "int", nullable: false),
					CarId = table.Column<int>(type: "int", nullable: false),
					RentDays = table.Column<int>(type: "int", nullable: false),
					Price = table.Column<int>(type: "int", nullable: false),
					RentEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RentedCars", x => x.Id);
					table.ForeignKey(
						name: "FK_RentedCars_cars_CarId",
						column: x => x.CarId,
						principalTable: "cars",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_RentedCars_users_UserId",
						column: x => x.UserId,
						principalTable: "users",
						principalColumn: "Id",
						onDelete: ReferentialAction.NoAction);
				});

			migrationBuilder.CreateTable(
				name: "UserLikedCar",
				columns: table => new
				{
					UserId = table.Column<int>(type: "int", nullable: false),
					CarId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserLikedCar", x => new { x.UserId, x.CarId });
					table.ForeignKey(
						name: "FK_UserLikedCar_cars_CarId",
						column: x => x.CarId,
						principalTable: "cars",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_UserLikedCar_users_UserId",
						column: x => x.UserId,
						principalTable: "users",
						principalColumn: "Id",
						onDelete: ReferentialAction.NoAction);
				});

			migrationBuilder.CreateIndex(
				name: "IX_CarImages_CarsId",
				table: "CarImages",
				column: "CarsId");

			migrationBuilder.CreateIndex(
				name: "IX_carModels_BrandId",
				table: "carModels",
				column: "BrandId");

			migrationBuilder.CreateIndex(
				name: "IX_cars_BrandId",
				table: "cars",
				column: "BrandId");

			migrationBuilder.CreateIndex(
				name: "IX_cars_ModelId",
				table: "cars",
				column: "ModelId");

			migrationBuilder.CreateIndex(
				name: "IX_cars_TransmissionId",
				table: "cars",
				column: "TransmissionId");

			migrationBuilder.CreateIndex(
				name: "IX_cars_UserId",
				table: "cars",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_RentedCars_CarId",
				table: "RentedCars",
				column: "CarId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_RentedCars_UserId",
				table: "RentedCars",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_UserLikedCar_CarId",
				table: "UserLikedCar",
				column: "CarId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "CarImages");

			migrationBuilder.DropTable(
				name: "RentedCars");

			migrationBuilder.DropTable(
				name: "UserLikedCar");

			migrationBuilder.DropTable(
				name: "cars");

			migrationBuilder.DropTable(
				name: "Transmissions");

			migrationBuilder.DropTable(
				name: "carModels");

			migrationBuilder.DropTable(
				name: "users");

			migrationBuilder.DropTable(
				name: "Brands");
		}
	}
}
