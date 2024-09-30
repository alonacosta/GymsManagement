using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressToGym : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Gyms");
        }
    }
}
