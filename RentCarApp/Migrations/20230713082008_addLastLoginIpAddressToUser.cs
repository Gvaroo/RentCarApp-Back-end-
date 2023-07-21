using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarApp.Migrations
{
    /// <inheritdoc />
    public partial class addLastLoginIpAddressToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastLoginIpAddress",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginIpAddress",
                table: "users");
        }
    }
}
