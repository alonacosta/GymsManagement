using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixDiscussions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussion_Post_OriginalPostId",
                table: "Discussion");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Discussion_DiscussionId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "UserViewModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discussion",
                table: "Discussion");

            migrationBuilder.RenameTable(
                name: "Discussion",
                newName: "Discussions");

            migrationBuilder.RenameIndex(
                name: "IX_Discussion_OriginalPostId",
                table: "Discussions",
                newName: "IX_Discussions_OriginalPostId");

            migrationBuilder.AlterColumn<int>(
                name: "GymId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GymId",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discussions",
                table: "Discussions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_Post_OriginalPostId",
                table: "Discussions",
                column: "OriginalPostId",
                principalTable: "Post",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Discussions_DiscussionId",
                table: "Post",
                column: "DiscussionId",
                principalTable: "Discussions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_Post_OriginalPostId",
                table: "Discussions");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Discussions_DiscussionId",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discussions",
                table: "Discussions");

            migrationBuilder.RenameTable(
                name: "Discussions",
                newName: "Discussion");

            migrationBuilder.RenameIndex(
                name: "IX_Discussions_OriginalPostId",
                table: "Discussion",
                newName: "IX_Discussion_OriginalPostId");

            migrationBuilder.AlterColumn<int>(
                name: "GymId",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GymId",
                table: "Clients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discussion",
                table: "Discussion",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserViewModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserViewModel", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Discussion_Post_OriginalPostId",
                table: "Discussion",
                column: "OriginalPostId",
                principalTable: "Post",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Discussion_DiscussionId",
                table: "Post",
                column: "DiscussionId",
                principalTable: "Discussion",
                principalColumn: "Id");
        }
    }
}
