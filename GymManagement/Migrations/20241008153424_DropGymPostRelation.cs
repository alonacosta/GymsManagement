using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.Migrations
{
    /// <inheritdoc />
    public partial class DropGymPostRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Gyms_GymId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_GymId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "GymId",
                table: "Post");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GymId",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Post_GymId",
                table: "Post",
                column: "GymId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Gyms_GymId",
                table: "Post",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id");
        }
    }
}
